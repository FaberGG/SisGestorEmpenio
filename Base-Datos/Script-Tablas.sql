Drop TABLE Administrador CASCADE CONSTRAINTS;
Drop TABLE Cliente CASCADE CONSTRAINTS;
Drop TABLE Articulo CASCADE CONSTRAINTS;
Drop TABLE Prestamo CASCADE CONSTRAINTS;
Drop TABLE Posee CASCADE CONSTRAINTS;
Drop TABLE Devolucion CASCADE CONSTRAINTS;


--CREACION DE TABLAS


CREATE TABLE Administrador (
    numeroIdentidad INT PRIMARY KEY,
    nombre VARCHAR2(30) NOT NULL,
    tipoIdentidad VARCHAR2(30) NOT NULL,
    salario DECIMAL(18,2) NOT NULL,
    aniosExperiencia INT NOT NULL,
    usuario VARCHAR(20),
    contrasenia VARCHAR (20)
);

CREATE TABLE Cliente (
    numeroIdentidad INT PRIMARY KEY,
    nombre VARCHAR2(30) NOT NULL,
    apellido VARCHAR(30) NOT NULL,
    telefono VARCHAR(18) NOT NULL,
    correo VARCHAR (40) NOT NULL,
    tipoIdentidad VARCHAR2(30) NOT NULL,
    numeroIdentidadAdministrador INT,
    FOREIGN KEY (numeroIdentidadAdministrador) REFERENCES Administrador(numeroIdentidad)
);

CREATE TABLE Articulo (
    idArticulo INT PRIMARY KEY,
    descripcion VARCHAR2 (100) NOT NULL,
    valorEstimado DECIMAL(18,2) NOT NULL,
    estadoArticulo VARCHAR2(12) NOT NULL,
    propiedadCasa NUMBER(1),
    estadoDevolucion VARCHAR2(12),
    CHECK (propiedadCasa IN (0, 1)), -- 0 = No es propiedad de la casa, 1 = Es propiedad de la casa
    CHECK (estadoDevolucion IN ('devuelto', 'libre')),
    CHECK (estadoArticulo IN ('defectuoso', 'optimo', 'funcionable'))
);

CREATE TABLE Prestamo (
    numeroIdentidadCliente INT,
    idArticulo INT,
    estadoPrestamo VARCHAR2(10) NOT NULL,
    fechaInicio DATE NOT NULL,
    fechaFin DATE NOT NULL,
    tasaInteres DECIMAL(5,2) NOT NULL,--5 digitos totales - 2 son decimales (100,00%)
    montoTotal DECIMAL (18,2),
    PRIMARY KEY (numeroIdentidadCliente, idArticulo),
    FOREIGN KEY (numeroIdentidadCliente) REFERENCES Cliente(numeroIdentidad),
    FOREIGN KEY (idArticulo) REFERENCES Articulo(idArticulo),
    CHECK (estadoPrestamo IN ('activo', 'inactivo'))
);

CREATE TABLE Posee (
    numeroIdentidadCliente INT,
    idArticulo INT,
    PRIMARY KEY (numeroIdentidadCliente, idArticulo),
    FOREIGN KEY (numeroIdentidadCliente) REFERENCES Cliente(numeroIdentidad),
    FOREIGN KEY (idArticulo) REFERENCES Articulo(idArticulo)
);

CREATE TABLE Devolucion (
    numeroIdentidadCliente INT,
    idArticulo INT,
    numConvenio INT,
    fechaDevolucion DATE NOT NULL,
    montoPagado DECIMAL (18,2) NOT NULL,
    PRIMARY KEY (numeroIdentidadCliente, idArticulo),
    FOREIGN KEY (numeroIdentidadCliente, idArticulo) REFERENCES Prestamo(numeroIdentidadCliente, idArticulo)
);


-------------------------------------------------------------------------------
--TRIGGER PARA EL AUTOINCREMENT DE DEVOLUCION
--------------------------------------------------------------------------------
CREATE OR REPLACE TRIGGER before_insert_devolucion
BEFORE INSERT ON Devolucion
FOR EACH ROW
DECLARE
    max_convenio NUMBER;
BEGIN
    -- Buscar el convenio más alto para ese cliente y artículo
    SELECT NVL(MAX(numConvenio), 0)
    INTO max_convenio
    FROM Devolucion
    WHERE numeroIdentidadCliente = :NEW.numeroIdentidadCliente
      AND idArticulo = :NEW.idArticulo;

    -- Asignar el nuevo número de convenio
    :NEW.numConvenio := max_convenio + 1;
END;
/


-------------------------------------------------------------------------------
--TRIGGER PARA ACTUALIZAR EL ESTADO DEL PRESTAMO Y ARTICULO AL REALIZAR UNA DEVOLUCION
--al regisrar una devolución, se verifica que el préstamo esté activo y se actualiza
-------------------------------------------------------------------------------

CREATE OR REPLACE TRIGGER trg_actualizar_estado_prestamo_y_articulo
AFTER INSERT ON Devolucion
FOR EACH ROW
DECLARE
    v_estado_empeno VARCHAR2(20);
BEGIN
    -- Verificar que el préstamo esté activo
    SELECT estadoPrestamo
    INTO v_estado_empeno
    FROM Prestamo
    WHERE numeroIdentidadCliente = :NEW.numeroIdentidadCliente
      AND idArticulo = :NEW.idArticulo;

    IF v_estado_empeno != 'activo' THEN
        RAISE_APPLICATION_ERROR(-20001, 'El préstamo no está activo y no puede ser devuelto.');
    END IF;

    -- Cambiar estado del préstamo a 'inactivo'
    UPDATE Prestamo
    SET estadoPrestamo = 'inactivo'
    WHERE numeroIdentidadCliente = :NEW.numeroIdentidadCliente
      AND idArticulo = :NEW.idArticulo;

    -- Actualizar el estado de devolución del artículo
    UPDATE Articulo
    SET estadoDevolucion = 'devuelto'
    WHERE idArticulo = :NEW.idArticulo;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20002, 'No existe un préstamo registrado para ese cliente y artículo.');
END;
/


--INSERTAR UN ADMINISTRADOR PREDETERMINADO
INSERT into ADMINISTRADOR values (1, 'admin', 'C.C', 900, 1, 'admin', 'admin');
COMMIT;
/



-------------------------------------------------------------------------------
--PROCEDIMIENTO PARA ACTUALIZAR ARTICULOS ADJUDICADOS
-- Este procedimiento actualiza los artículos adjudicados que no han sido devueltos
-- y los marca como adjudicados, además de establecer su propiedad en la casa.
-- Se ejecuta diariamente a la medianoche.
------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE actualizar_articulos_adjudicados AS
BEGIN
    UPDATE Articulo
    SET propiedadCasa = 1,
        estadoDevolucion = 'adjudicado'
    WHERE idArticulo IN (
        SELECT idArticulo
        FROM Prestamo
        WHERE fechaFin < SYSDATE
          AND estadoPrestamo = 'activo'
    )
    AND estadoDevolucion != 'devuelto'
    AND propiedadCasa = 0;

    COMMIT;
END;
/
-------------------------------------------------------------------------------
--ASEGURATE DE TENER LOS PRIVILEGIOS NECESARIOS PARA CREAR JOBS
-------------------------------------------------------------------------------

--GRANT CREATE JOB TO tu_usuario;
GRANT CREATE JOB TO BDD1;
/



--este procedimiento se ejecuta diariamente a la medianoche
--CREACION DEL JOB PARA ACTUALIZAR ARTICULOS ADJUDICADOS
BEGIN
    DBMS_SCHEDULER.CREATE_JOB (
        job_name        => 'JOB_ACTUALIZAR_ARTICULOS_ADJUDICADOS',
        job_type        => 'STORED_PROCEDURE',
        job_action      => 'ACTUALIZAR_ARTICULOS_ADJUDICADOS',
        start_date      => SYSTIMESTAMP,
        repeat_interval => 'FREQ=DAILY;BYHOUR=0;BYMINUTE=0;BYSECOND=0',
        enabled         => TRUE,
        auto_drop       => FALSE
    );
END;
/



--------------------------------------------
--ALGUNAS CONSULTAS Y PROCEDIMIENTOS UTILES
--------------------------------------------

--prestamo por cliente
SELECT * FROM Prestamo;

SELECT * from ARTICULO;
SELECT * FROM Prestamo WHERE numeroIdentidadCliente = 1;
--prestamos por articulo
SELECT * FROM Prestamo WHERE idArticulo = 1;
--articulos por cliente
SELECT * FROM Posee WHERE numeroIdentidadCliente = 1;
--articulos por articulo
SELECT * FROM Posee WHERE idArticulo = 1;

