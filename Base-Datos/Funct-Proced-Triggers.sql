


---------------------------------------------------------
---------------------------------------------------------

--
--Todos estos procedimientos-FINC-TRGG no estan implementados con el
--software. Se deben re-escribir para que coincidan con la
--base de datos que estamos manejando. 
--POR EL MOMENTO SON SOLO IDEAS SIN IMPLEMENTAR REALMENTE
--

---------------------------------------------------------
---------------------------------------------------------















-------------------------------------------------------------------------------------------------------------
--2.Procedimiento para realizar una devolución completa, permite registrar la devolución cambiando el estado
--del préstamo asociado
-------------------------------------------------------------------------------------------------------------

CREATE OR REPLACE PROCEDURE registrar_devolucion_completa(
 p_numero_identidad INT,
 p_id_articulo INT,
 p_fecha_devolucion DATE
)
IS
 v_estado_empeno VARCHAR2(20);
BEGIN
 -- Verificar si existe el préstamo y si está en estado 'activo'
     SELECT estadoPrestamo
     INTO v_estado_empeno
     FROM Empeno
     WHERE numeroIdentidadCliente = p_numero_identidad
     AND idArticulo = p_id_articulo;
     IF v_estado_empeno != 'activo' THEN
     RAISE_APPLICATION_ERROR(-20001, 'El préstamo no está activo y no
    puede ser devuelto.');
     END IF;
 -- Insertar devolución (el trigger autogenera numConvenio)
    INSERT INTO Devolucion (
         numeroIdentidadCliente,
         idArticulo,
         fechaDevolucion
     ) VALUES (
         p_numero_identidad,
         p_id_articulo,
         p_fecha_devolucion
     );
 -- Actualizar estado del préstamo a 'devuelto'
     UPDATE Empeno
     SET estadoPrestamo = 'devuelto'
     WHERE numeroIdentidadCliente = p_numero_identidad
        AND idArticulo = p_id_articulo;
        DBMS_OUTPUT.PUT_LINE('Devolucion registrada con exito');
    EXCEPTION
         WHEN NO_DATA_FOUND THEN
             RAISE_APPLICATION_ERROR(-20002, 'No existe un préstamo registrado
                çpara ese cliente y artículo.');
     WHEN DUP_VAL_ON_INDEX THEN
        RAISE_APPLICATION_ERROR(-20003, 'Ya existe una devolución registrada
        con la misma combinación de cliente y artículo.');
     WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20099, 'Error inesperado: ' || SQLERRM);
END;

--Datos de prueba
BEGIN
    registrar_devolucion_completa(77889911, 11, SYSDATE);
END;
BEGIN
    registrar_devolucion_completa(77889900, 9, SYSDATE);
END;

--Ejecucion
BEGIN
 eliminar_cliente_sin_empenos(9999);
END;





-------------------------------------------------------------------------------------------------------------
--4. Diseñe una función para realizar el proceso de liquidación a aquellos empeños vencidos 
--(no se hizo la devolución en la fecha estimada), cambiando su estado a inactivo. La función debe 
--retornar el número de préstamos liquidados por vencimiento.

-------------------------------------------------------------------------------------------------------------
-- Procedimiento para Liquidación de Empeños Vencidos
drop FUNCTION fn_liquidar_empenos_vencidos;
CREATE OR REPLACE FUNCTION fn_liquidar_empenos_vencidos (
    p_dias_grace_period IN NUMBER DEFAULT 0
) RETURN NUMBER
IS
    TYPE t_empenos_vencidos IS TABLE OF Empeno%ROWTYPE;
    v_empenos t_empenos_vencidos;

    v_fecha_actual DATE := SYSDATE;
    v_fecha_limite DATE := v_fecha_actual - p_dias_grace_period;

    v_cantidad_procesados NUMBER := 0;
BEGIN
    -- Validación del parámetro de entrada
    IF p_dias_grace_period IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'El parámetro p_dias_grace_period no puede ser NULL');
    END IF;

    -- Paso 1: Recolectar empeños vencidos
    SELECT *
    BULK COLLECT INTO v_empenos
    FROM Empeno
    WHERE estadoPrestamo = 'activo'
    AND fechaFin < v_fecha_limite;

    -- Paso 2: Actualizar si hay registros
    IF v_empenos.COUNT > 0 THEN
        FORALL i IN 1..v_empenos.COUNT
            UPDATE Empeno
            SET estadoPrestamo = 'inactivo'
            WHERE numeroIdentidadCliente = v_empenos(i).numeroIdentidadCliente
            AND idArticulo = v_empenos(i).idArticulo;

        v_cantidad_procesados := v_empenos.COUNT;
    END IF;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Proceso completado. Empeños liquidados: ' || v_cantidad_procesados);
    RETURN v_cantidad_procesados;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('No se encontraron empeños vencidos.');
        RETURN 0;
    WHEN VALUE_ERROR THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error de tipo de dato. Parámetro inválido.');
        RAISE;
    WHEN TOO_MANY_ROWS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error inesperado: demasiadas filas.');
        RAISE;
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error general: ' || SQLERRM);
        RAISE;
END fn_liquidar_empenos_vencidos;


--datos para pruebas
-- Agencias y Administradores
INSERT INTO Administrador VALUES (101, 'Carlos Perez', 'DNI', 2500, 5, 'a', 'a');

-- Clientes
INSERT INTO Cliente VALUES (201, 'Juan', 'Gomez', 12, 'a', 'DNI', 101);
INSERT INTO Cliente VALUES (202, 'Maria', 'Lopez', 12, 'a', 'DNI', 101);

-- Artículos
INSERT INTO Articulo VALUES (301, 'Reloj', 200.00, 'optimo', 1, 'null');
INSERT INTO Articulo VALUES (302, 'Laptop', 1500.00, 'funcionable', 1, 'null');

-- Empeños
INSERT INTO PRESTAMO VALUES (201, 301, 'activo', SYSDATE - 30, SYSDATE - 10, 5.0, 0); -- vencido
INSERT INTO PRESTAMO VALUES (202, 302, 'activo', SYSDATE - 5, SYSDATE + 10, 5.0, 0);  -- no vencido


DECLARE
    v_resultado NUMBER;
BEGIN
    v_resultado := fn_liquidar_empenos_vencidos(5);
    DBMS_OUTPUT.PUT_LINE('Préstamos liquidados: ' || v_resultado);
END;