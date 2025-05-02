## Release v1.0.1 - Funcionalidades Iniciales y Configuración de Base de Datos

Esta es la primera versión pública del Sistema de Gestión de Préstamos, que introduce las funcionalidades básicas para la gestión inicial.

**Contenido:**

* [Configuración de Base de Datos](#se-agregó-el-archivo-de-configuracion-appconfig-para-configurar-la-conexion-con-la-base-de-datos-desde-la-version-distribuible)
* [Funcionalidades Soportadas en esta Versión](#funcionalidades-soportadas-en-esta-version)
    * [Pantalla de Log-In](#pantalla-de-log-in)
    * [Registrar Préstamo, Cliente y Artículo](#registrar-prestamo-cliente-y-articulo)
        * [Validaciones de Campos](#para-cada-campo-de-texto-realiza-las-validaciones-necesarias-para-asegurar-que-los-datos-ingresados-sean-correctos)
    * [Registrar Devolución de un Préstamo](#registrar-devolucion-de-un-prestamo)
* [Instrucciones de Instalación](#instrucciones-de-instalacion)
* [Problemas Conocidos](#problemas-conocidos)

---

###   Se agregó el archivo de configuracion app.config para configurar la conexion con la base de datos desde la version distribuible
![image](https://github.com/user-attachments/assets/2309ac68-8b76-462e-a0d6-e3bcdc9bd0ca)

# Funcionalidades soportadas en esta version (desde la v1.0)
## Pantalla de Log-In
Solo un **administrador ya registrado** en la base de datos puede ingresar a hacer uso del software
Ingresa sus credenciales (quizá otorgadas por el contratista o quien lo registró) para usar el software
![image](https://github.com/user-attachments/assets/0b88d53d-1605-4c41-9247-5f34544a7834)

## Registrar prestamo, cliente y articulo
Se realiza el registro de un préstamo solicitando los datos del cliente y los datos del artículo a registrar
Es posible registrar un prestamo con un cliente o un articulo previamente registrado
![image](https://github.com/user-attachments/assets/ff76bae7-1fc0-40b7-be49-a7ccbec1443b)
![image](https://github.com/user-attachments/assets/10d4034c-49a5-494b-ac59-3345d776933a)

### Para cada campo de texto realiza las validaciones necesarias para asegurar que los datos ingresados sean correctos
1.  validación campos vacíos
2.  validación límite de texto ingresado en un campo
3.  validación campos numéricos sean correctos (decimales y enteros)
4.  validación formato en correo, teléfono, tasa de interés en porcentaje
5.  validación cliente o artículo ya registrado (duplicado)

![image](https://github.com/user-attachments/assets/746ee6c9-36e0-47b8-816b-d516e87eff1d)
![image](https://github.com/user-attachments/assets/7ace5c11-35b9-4dee-b9b7-8201e53937b0)
![image](https://github.com/user-attachments/assets/fd639ce8-1647-4bc6-9f8a-4151ed063e81)
![image](https://github.com/user-attachments/assets/29704984-04d8-42c6-834a-23d908e0193a)

## Registrar devolucion de un prestamo
Al registrar exitosamente la devolucion de un prestamo el estado de devolucion del articulo asociado para a "devuelto" y el prestamo a un estado de "Inactivo"
se valida que
1.  el prestamo exista para poder realizar la devolucion
2.  la devolucion no se haya hecho anteriormente
3.  validacion en los campos anteriormente descritos
![image](https://github.com/user-attachments/assets/2957e26f-bcc9-4f54-9a59-030273a211ec)
![image](https://github.com/user-attachments/assets/8eef6aca-a9b1-440d-b120-fc40e0e3b712)
### El estado del articulo pasa a devuelto y el del prestamo a inactivo **al realizar la devolucion**
![image](https://github.com/user-attachments/assets/4f7b4a48-b6e5-4dfb-8a5c-b6281d005837)
![image](https://github.com/user-attachments/assets/09b71173-cab4-4d0d-86b8-dd88798427cc)

## Instrucciones de Instalación
1.  Descargue el archivo ejecutable desde la sección "Assets" de esta página..
2.  Extraiga la carpeta y configure la conexión a la base de datos editando el archivo `SisGestorEmpenio.dll.config`:
    * `Server=mi_servidor;Database=mi_base_de_datos;User Id=mi_usuario;Password=mi_contraseña;`
4.  Ejecute el archivo de aplicacion SisGestorEmpenio.exe.
