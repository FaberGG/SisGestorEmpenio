-- *** Script para la Base de Datos Oracle XE ***
-- Asume que estás conectado a la PDB (por ejemplo, XEPDB1) como SYSTEM o un usuario con privilegios DBA.

-- 1. Crear el usuario DEVELOPER
-- Este usuario tendrá privilegios para DML, DDL y DCL básicos (sobre sus propios objetos).
CREATE USER DEV IDENTIFIED BY oracle;

-- 2. Crear el usuario TESTER
-- Este usuario tendrá privilegios para DML (SELECT, INSERT, UPDATE, DELETE)
-- pero NO para DDL (CREATE, ALTER, DROP).
CREATE USER TESTER IDENTIFIED BY tester_password; -- Cambia 'tester_password' por una contraseña segura

-- 3. Crear roles personalizados para mayor control
-- Rol para Desarrolladores: Permite DML, DDL en su propio esquema
CREATE ROLE ROL_DEVELOPER;

-- Rol para Testers: Permite DML (SELECT, INSERT, UPDATE, DELETE)
CREATE ROLE ROL_TESTER;

-- 4. Otorgar privilegios a los roles

-- Privilegios para ROL_DEVELOPER:
-- - Conexión a la base de datos
-- - Creación de objetos (tablas, vistas, secuencias, procedimientos, etc.) en su propio esquema
-- - DML básico (SELECT, INSERT, UPDATE, DELETE) en sus propios objetos
-- - Vistas
-- - Uso de tablespace ilimitado (considera asignar una cuota específica en un entorno de producción)
GRANT CONNECT, RESOURCE TO ROL_DEVELOPER;
GRANT CREATE VIEW TO ROL_DEVELOPER;
GRANT UNLIMITED TABLESPACE TO ROL_DEVELOPER;


-- Privilegios para ROL_TESTER:
-- - Conexión a la base de datos
-- - DML (SELECT, INSERT, UPDATE, DELETE)
-- El rol CONNECT ya incluye CREATE SESSION.
GRANT CONNECT TO ROL_TESTER;

-- Para permitir DML en tablas ESPECÍFICAS creadas por el desarrollador,
-- el TESTER necesitará privilegios sobre esas tablas.
-- Esto debe hacerse DESPUÉS de que el DEVELOPER haya creado las tablas.
-- Ejemplo (ejecutar por SYSTEM o el DEVELOPER una vez que la tabla exista):
-- GRANT SELECT, INSERT, UPDATE, DELETE ON DEV.NOMBRE_TABLA TO ROL_TESTER;


-- 5. Asignar los roles a los usuarios

-- Asignar ROL_DEVELOPER al usuario DEV
GRANT ROL_DEVELOPER TO DEV;

-- Asignar ROL_TESTER al usuario TESTER
GRANT ROL_TESTER TO TESTER;

-- 6. Opcional: Revocar privilegios básicos si se usaron roles predefinidos (no aplica directamente aquí, pero es una buena práctica)
-- Por ejemplo, si inicialmente hubieras dado RESOURCE al TESTER y ahora quieres refinarlo.
-- REVOKE RESOURCE FROM TESTER;