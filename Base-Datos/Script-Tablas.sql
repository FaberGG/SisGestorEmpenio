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


--TRIGGER PARA EL AUTOINCREMENT DE DEVOLUCION

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


--INSERTAR UN ADMINISTRADOR PREDETERMINADO
INSERT into ADMINISTRADOR values (1, 'admin', 'C.C', 900, 1, 'admin', 'admin');
COMMIT;


--------------------------------------------
--ALGUNAS CONSULTAS Y PROCEDIMIENTOS UTILES
--------------------------------------------

--prestamo por cliente
SELECT * FROM Prestamo
WHERE numeroIdentidadCliente = 12345678;

