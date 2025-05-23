-- INSERTS DE PRUEBA PARA SISTEMA DE PRÉSTAMOS

-- ========================================
-- 1. INSERTAR ADMINISTRADORES
-- ========================================

INSERT INTO Administrador (numeroIdentidad, nombre, tipoIdentidad, salario, aniosExperiencia, usuario, contrasenia) 
VALUES (12345678, 'Maria Rodriguez', 'Cedula', 3500000.00, 5, 'mrodriguez', 'admin123');

INSERT INTO Administrador (numeroIdentidad, nombre, tipoIdentidad, salario, aniosExperiencia, usuario, contrasenia) 
VALUES (23456789, 'Carlos Mendez', 'Cedula', 4200000.00, 8, 'cmendez', 'admin456');

INSERT INTO Administrador (numeroIdentidad, nombre, tipoIdentidad, salario, aniosExperiencia, usuario, contrasenia) 
VALUES (34567890, 'Ana Gutierrez', 'Cedula', 3800000.00, 3, 'agutierrez', 'admin789');

-- ========================================
-- 2. INSERTAR CLIENTES
-- ========================================

INSERT INTO Cliente (numeroIdentidad, nombre, apellido, telefono, correo, tipoIdentidad, numeroIdentidadAdministrador) 
VALUES (87654321, 'Juan', 'Perez', '3001234567', 'juan.perez@email.com', 'Cedula', 12345678);

INSERT INTO Cliente (numeroIdentidad, nombre, apellido, telefono, correo, tipoIdentidad, numeroIdentidadAdministrador) 
VALUES (76543210, 'Laura', 'Martinez', '3109876543', 'laura.martinez@email.com', 'Cedula', 12345678);

INSERT INTO Cliente (numeroIdentidad, nombre, apellido, telefono, correo, tipoIdentidad, numeroIdentidadAdministrador) 
VALUES (65432109, 'Pedro', 'Gonzalez', '3201112233', 'pedro.gonzalez@email.com', 'Cedula', 23456789);

INSERT INTO Cliente (numeroIdentidad, nombre, apellido, telefono, correo, tipoIdentidad, numeroIdentidadAdministrador) 
VALUES (54321098, 'Sofia', 'Lopez', '3154445566', 'sofia.lopez@email.com', 'Cedula', 23456789);

INSERT INTO Cliente (numeroIdentidad, nombre, apellido, telefono, correo, tipoIdentidad, numeroIdentidadAdministrador) 
VALUES (43210987, 'Miguel', 'Ramirez', '3187778899', 'miguel.ramirez@email.com', 'Cedula', 34567890);

INSERT INTO Cliente (numeroIdentidad, nombre, apellido, telefono, correo, tipoIdentidad, numeroIdentidadAdministrador) 
VALUES (32109876, 'Carmen', 'Torres', '3166669900', 'carmen.torres@email.com', 'Cedula', 34567890);

-- ========================================
-- 3. INSERTAR ARTÍCULOS (PRODUCTOS)
-- ========================================

-- Artículos para préstamos múltiples del primer cliente
INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1001, 'Anillo de oro 18k con diamante', 2500000.00, 'optimo', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1002, 'Collar de perlas naturales', 1800000.00, 'optimo', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1003, 'Reloj Rolex Submariner', 8500000.00, 'funcionable', 0, NULL);

-- Artículos para otros clientes
INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1004, 'Cadena de oro de 14k', 1200000.00, 'optimo', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1005, 'Aretes de esmeraldas', 3200000.00, 'optimo', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1006, 'Pulsera de plata con zafiros', 950000.00, 'funcionable', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1007, 'Televisor Samsung 55 pulgadas', 2800000.00, 'optimo', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1008, 'Laptop MacBook Pro', 6500000.00, 'funcionable', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1009, 'Guitarra eléctrica Fender', 2200000.00, 'optimo', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1010, 'Tablet iPad Air', 1800000.00, 'defectuoso', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1011, 'Cámara Canon EOS R5', 7200000.00, 'optimo', 0, NULL);

INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion) 
VALUES (1012, 'Consola PlayStation 5', 2500000.00, 'funcionable', 0, NULL);

-- ========================================
-- 4. INSERTAR RELACIÓN POSEE (PROPIEDAD)
-- ========================================

-- Cliente Juan Perez posee múltiples artículos
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (87654321, 1001);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (87654321, 1002);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (87654321, 1003);

-- Otros clientes poseen artículos
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (76543210, 1004);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (76543210, 1005);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (65432109, 1006);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (65432109, 1007);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (54321098, 1008);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (54321098, 1009);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (43210987, 1010);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (43210987, 1011);
INSERT INTO Posee (numeroIdentidadCliente, idArticulo) VALUES (32109876, 1012);

-- ========================================
-- 5. INSERTAR PRÉSTAMOS
-- ========================================

-- MÚLTIPLES PRÉSTAMOS PARA JUAN PEREZ (Cliente: 87654321)
INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (87654321, 1001, 'activo', DATE '2024-01-15', DATE '2024-07-15', 15.50, 1875000.00);

INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (87654321, 1002, 'inactivo', DATE '2024-02-10', DATE '2024-08-10', 12.00, 1350000.00);

INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (87654321, 1003, 'activo', DATE '2024-03-05', DATE '2024-09-05', 18.75, 6375000.00);

-- PRÉSTAMOS PARA LAURA MARTINEZ (Cliente: 76543210)
INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (76543210, 1004, 'inactivo', DATE '2024-01-20', DATE '2024-07-20', 14.00, 900000.00);

INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (76543210, 1005, 'activo', DATE '2024-04-01', DATE '2024-10-01', 16.25, 2400000.00);

-- PRÉSTAMOS PARA PEDRO GONZALEZ (Cliente: 65432109)
INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (65432109, 1006, 'inactivo', DATE '2024-02-15', DATE '2024-08-15', 13.50, 712500.00);

INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (65432109, 1007, 'activo', DATE '2024-05-10', DATE '2024-11-10', 17.00, 2100000.00);

-- PRÉSTAMOS PARA SOFIA LOPEZ (Cliente: 54321098)
INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (54321098, 1008, 'activo', DATE '2024-03-20', DATE '2024-09-20', 19.50, 4875000.00);

INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (54321098, 1009, 'inactivo', DATE '2024-01-25', DATE '2024-07-25', 15.00, 1650000.00);

-- PRÉSTAMOS PARA MIGUEL RAMIREZ (Cliente: 43210987)
INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (43210987, 1010, 'activo', DATE '2024-04-15', DATE '2024-10-15', 11.50, 1350000.00);

INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (43210987, 1011, 'inactivo', DATE '2024-02-28', DATE '2024-08-28', 20.00, 5400000.00);

-- PRÉSTAMO PARA CARMEN TORRES (Cliente: 32109876)
INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (32109876, 1012, 'activo', DATE '2024-05-01', DATE '2024-11-01', 16.75, 1875000.00);

-- ========================================
-- 6. INSERTAR DEVOLUCIONES (UNA POR PRÉSTAMO)
-- ========================================

-- DEVOLUCIÓN PARA JUAN PEREZ 
-- Devolución completa del collar (artículo 1002)
INSERT INTO Devolucion (numeroIdentidadCliente, idArticulo, fechaDevolucion, montoPagado) 
VALUES (87654321, 1002, DATE '2024-06-10', 1350000.00);

-- DEVOLUCIÓN PARA LAURA MARTINEZ
-- Devolución completa de la cadena (artículo 1004)
INSERT INTO Devolucion (numeroIdentidadCliente, idArticulo, fechaDevolucion, montoPagado) 
VALUES (76543210, 1004, DATE '2024-05-20', 900000.00);

-- DEVOLUCIÓN PARA PEDRO GONZALEZ
-- Devolución completa de la pulsera (artículo 1006)
INSERT INTO Devolucion (numeroIdentidadCliente, idArticulo, fechaDevolucion, montoPagado) 
VALUES (65432109, 1006, DATE '2024-07-15', 712500.00);

-- DEVOLUCIÓN PARA SOFIA LOPEZ
-- Devolución parcial de la guitarra (artículo 1009)
INSERT INTO Devolucion (numeroIdentidadCliente, idArticulo, fechaDevolucion, montoPagado) 
VALUES (54321098, 1009, DATE '2024-04-30', 825000.00);

-- DEVOLUCIÓN PARA MIGUEL RAMIREZ
-- Devolución completa de la cámara (artículo 1011)
INSERT INTO Devolucion (numeroIdentidadCliente, idArticulo, fechaDevolucion, montoPagado) 
VALUES (43210987, 1011, DATE '2024-07-30', 5400000.00);

COMMIT;

SELECT * from PRESTAMO where IDARTICULO = 1001;
SELECT * from Articulo where IDARTICULO = 1001;
INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal) 
VALUES (32109876, 1001, 'activo', DATE '2024-05-01', DATE '2024-11-01', 16.75, 1875000.00);

-- ========================================
-- RESUMEN DE DATOS INSERTADOS:
-- ========================================
-- 3 Administradores
-- 6 Clientes
-- 12 Artículos
-- 13 Préstamos (Juan Pérez tiene 3, otros clientes tienen entre 1-2)
-- 5 Devoluciones (una por préstamo, sin duplicados)
-- Algunos préstamos están activos, otros inactivos
-- Las devoluciones incluyen pagos completos y parciales
COMMIT;

-- ========================================

-- BLOQUE PL/SQL SIMPLIFICADO PARA GENERAR 5000 PRÉSTAMOS

DECLARE
    v_cliente_id NUMBER := 100000;  -- Inicio de IDs de clientes
    v_articulo_id NUMBER := 2000;   -- Inicio de IDs de artículos
    v_cantidad_prestamos NUMBER := 50000; -- Cantidad de préstamos a generar
BEGIN
    DBMS_OUTPUT.PUT_LINE('Iniciando generación de ' || v_cantidad_prestamos || ' préstamos...');
    INSERT INTO Administrador (numeroIdentidad, nombre, tipoIdentidad, salario, aniosExperiencia, usuario, contrasenia) 
    VALUES (23432343, 'Maria Rodriguez', 'Cedula', 3500000.00, 5, 'mrodriguez', 'admin123');
    -- Loop para crear n  préstamos
    FOR i IN 1..v_cantidad_prestamos LOOP
        
        -- Insertar cliente (datos fijos, solo cambia ID)
        INSERT INTO Cliente (numeroIdentidad, nombre, apellido, telefono, correo, tipoIdentidad, numeroIdentidadAdministrador)
        VALUES (v_cliente_id, 'Cliente', 'Numero' || i, '3001234567', 'cliente' || i || '@email.com', 'Cedula', 23432343);
        
        -- Insertar artículo (datos fijos, solo cambia ID)
        INSERT INTO Articulo (idArticulo, descripcion, valorEstimado, estadoArticulo, propiedadCasa, estadoDevolucion)
        VALUES (v_articulo_id, 'Articulo numero ' || i, 1000000.00, 'optimo', 0, NULL);
        
        -- Insertar relación de propiedad
        INSERT INTO Posee (numeroIdentidadCliente, idArticulo)
        VALUES (v_cliente_id, v_articulo_id);
        
        -- Insertar préstamo (datos fijos, solo cambian IDs)
        INSERT INTO Prestamo (numeroIdentidadCliente, idArticulo, estadoPrestamo, fechaInicio, fechaFin, tasaInteres, montoTotal)
        VALUES (v_cliente_id, v_articulo_id, 'activo', DATE '2024-01-01', DATE '2024-07-01', 15.00, 750000.00);
        
        -- Incrementar contadores
        v_cliente_id := v_cliente_id + 1;
        v_articulo_id := v_articulo_id + 1;
        
        -- Mostrar progreso cada 1000 registros
        IF MOD(i, 1000) = 0 THEN
            DBMS_OUTPUT.PUT_LINE('Procesados ' || i || ' préstamos...');
            COMMIT;
        END IF;
        
    END LOOP;
    
    COMMIT;
    DBMS_OUTPUT.PUT_LINE('¡Generación completada!' || v_cantidad_prestamos || 'préstamos creados.');
    
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
END;
/
