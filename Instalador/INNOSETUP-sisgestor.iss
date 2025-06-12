; Script de Inno Setup para la aplicación SisGestorEmpenio

[Setup]
AppName=SisGestorEmpenio
AppVersion=1.0
DefaultDirName={autopf}\SisGestorEmpenio
DefaultGroupName=SisGestorEmpenio
OutputBaseFilename=SisGestorEmpenioInstaller
Compression=lzma
SolidCompression=yes
WizardStyle=modern
; Agregar versión mínima de Windows si es necesario
; MinVersion=10.0.17763
; Agregar arquitectura si es necesario
; ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\MaterialDesignColors.dll"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\MaterialDesignThemes.Wpf.dll"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\Microsoft.Xaml.Behaviors.dll"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\Oracle.ManagedDataAccess.dll"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\SisGestorEmpenio.deps.json"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\SisGestorEmpenio.dll"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\SisGestorEmpenio.dll.config"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\SisGestorEmpenio.exe"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\SisGestorEmpenio.pdb"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\SisGestorEmpenio.runtimeconfig.json"; DestDir: "{app}"
Source: "C:\Users\jhonn\OneDrive\Escritorio\SIs-Gestor-Empenio\System.DirectoryServices.Protocols.dll"; DestDir: "{app}"

[Icons]
Name: "{group}\SisGestorEmpenio"; Filename: "{app}\SisGestorEmpenio.exe"
Name: "{commondesktop}\SisGestorEmpenio"; Filename: "{app}\SisGestorEmpenio.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\SisGestorEmpenio.exe"; Description: "{cm:LaunchProgram,SisGestorEmpenio}"; Flags: nowait postinstall skipifdoesntexist

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[CustomMessages]
LaunchProgram=Lanzar %1
CreateDesktopIcon=Crear un icono en el escritorio
AdditionalIcons=Iconos Adicionales

[Code]
var
  PageOracleSettings: TWizardPage;
  LabelServer: TNewStaticText;
  EditServer: TNewEdit;
  LabelPort: TNewStaticText;
  EditPort: TNewEdit;
  LabelServiceName: TNewStaticText;
  EditServiceName: TNewEdit;
  LabelUser: TNewStaticText;
  EditUser: TNewEdit;
  LabelPassword: TNewStaticText;
  EditPassword: TNewEdit;
  DescriptionLabel: TNewStaticText;

// Función para validar que el archivo XML sea válido
function ValidateXmlFile(const FileName: string): Boolean;
var
  Lines: TArrayOfString;
  Content: string;
  I: Integer;
begin
  Result := True;
  if LoadStringsFromFile(FileName, Lines) then
  begin
    Content := '';
    for I := 0 to GetArrayLength(Lines) - 1 do
      Content := Content + Lines[I] + #13#10;
    
    // Validaciones básicas de XML
    if (Pos('<?xml', Content) = 0) and (Pos('<configuration', Content) = 0) then
    begin
      Log('ERROR: No se encontró declaración XML válida');
      Result := False;
    end;
    
    // Verificar que las etiquetas abran y cierren correctamente
    if (Pos('<configuration>', Content) > 0) and (Pos('</configuration>', Content) = 0) then
    begin
      Log('ERROR: Etiqueta configuration no cerrada correctamente');
      Result := False;
    end;
    
    // Verificar que no hay caracteres especiales sin escapar
    if (Pos('&', Content) > 0) and (Pos('&amp;', Content) = 0) and 
       (Pos('&lt;', Content) = 0) and (Pos('&gt;', Content) = 0) and
       (Pos('&quot;', Content) = 0) and (Pos('&apos;', Content) = 0) then
    begin
      Log('WARNING: Posibles caracteres especiales sin escapar en XML');
    end;
  end else
  begin
    Log('ERROR: No se pudo leer el archivo para validación');
    Result := False;
  end;
end;

// Función para escapar caracteres especiales en XML
function EscapeXmlString(const S: string): string;
var
  I: Integer;
  C: Char;
begin
  Result := '';
  for I := 1 to Length(S) do
  begin
    C := S[I];
    case C of
      '&': Result := Result + '&amp;';
      '<': Result := Result + '&lt;';
      '>': Result := Result + '&gt;';
      '"': Result := Result + '&quot;';
      '''': Result := Result + '&apos;';
    else
      Result := Result + C;
    end;
  end;
end;

procedure InitializeWizard;
begin
  // Crear una página personalizada para la configuración de la base de datos
  PageOracleSettings := CreateCustomPage(wpWelcome,
    'Configuración de la Base de Datos Oracle', 
    'Por favor, ingrese los detalles de conexión a Oracle DB XE.');

  // Añadir la descripción
  DescriptionLabel := TNewStaticText.Create(WizardForm);
  DescriptionLabel.Parent := PageOracleSettings.Surface;
  DescriptionLabel.Caption := 'Estos datos se usarán para conectar su programa a la base de datos.';
  DescriptionLabel.Left := 0;
  DescriptionLabel.Top := 5;
  DescriptionLabel.Width := PageOracleSettings.Surface.ClientWidth;
  DescriptionLabel.Height := 20;
  DescriptionLabel.Font.Style := [fsItalic];
  // Nota: Alignment no está disponible para TNewStaticText en Inno Setup
  // Para centrar texto, necesitarías calcular la posición manualmente

  // Controles para Servidor/Host
  LabelServer := TNewStaticText.Create(WizardForm);
  LabelServer.Parent := PageOracleSettings.Surface;
  LabelServer.Caption := 'Servidor/Host:';
  LabelServer.Left := 20;
  LabelServer.Top := 40;
  LabelServer.Width := 150;
  LabelServer.Height := 15;

  EditServer := TNewEdit.Create(WizardForm);
  EditServer.Parent := PageOracleSettings.Surface;
  EditServer.Left := 170;
  EditServer.Top := 38;
  EditServer.Width := 250;
  EditServer.Height := 21;
  EditServer.Text := 'localhost';

  // Controles para Puerto
  LabelPort := TNewStaticText.Create(WizardForm);
  LabelPort.Parent := PageOracleSettings.Surface;
  LabelPort.Caption := 'Puerto (usualmente 1521):';
  LabelPort.Left := 20;
  LabelPort.Top := 70;
  LabelPort.Width := 150;
  LabelPort.Height := 15;

  EditPort := TNewEdit.Create(WizardForm);
  EditPort.Parent := PageOracleSettings.Surface;
  EditPort.Left := 170;
  EditPort.Top := 68;
  EditPort.Width := 250;
  EditPort.Height := 21;
  EditPort.Text := '1521';

  // Controles para Service Name
  LabelServiceName := TNewStaticText.Create(WizardForm);
  LabelServiceName.Parent := PageOracleSettings.Surface;
  LabelServiceName.Caption := 'Service Name (o SID):';
  LabelServiceName.Left := 20;
  LabelServiceName.Top := 100;
  LabelServiceName.Width := 150;
  LabelServiceName.Height := 15;

  EditServiceName := TNewEdit.Create(WizardForm);
  EditServiceName.Parent := PageOracleSettings.Surface;
  EditServiceName.Left := 170;
  EditServiceName.Top := 98;
  EditServiceName.Width := 250;
  EditServiceName.Height := 21;
  EditServiceName.Text := 'XEPDB1';

  // Controles para Usuario
  LabelUser := TNewStaticText.Create(WizardForm);
  LabelUser.Parent := PageOracleSettings.Surface;
  LabelUser.Caption := 'Usuario de BD:';
  LabelUser.Left := 20;
  LabelUser.Top := 130;
  LabelUser.Width := 150;
  LabelUser.Height := 15;

  EditUser := TNewEdit.Create(WizardForm);
  EditUser.Parent := PageOracleSettings.Surface;
  EditUser.Left := 170;
  EditUser.Top := 128;
  EditUser.Width := 250;
  EditUser.Height := 21;
  EditUser.Text := 'tu_usuario_bd';

  // Controles para Contraseña
  LabelPassword := TNewStaticText.Create(WizardForm);
  LabelPassword.Parent := PageOracleSettings.Surface;
  LabelPassword.Caption := 'Contraseña de BD:';
  LabelPassword.Left := 20;
  LabelPassword.Top := 160;
  LabelPassword.Width := 150;
  LabelPassword.Height := 15;

  EditPassword := TNewEdit.Create(WizardForm);
  EditPassword.Parent := PageOracleSettings.Surface;
  EditPassword.Left := 170;
  EditPassword.Top := 158;
  EditPassword.Width := 250;
  EditPassword.Height := 21;
  EditPassword.Text := '';
  EditPassword.PasswordChar := '*';
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  AppConfigFile: string;
  OracleHost: string;
  OraclePort: string;
  OracleServiceName: string;
  OracleUser: string;
  OraclePassword: string;
  ConnectionString: string;
  Lines: TArrayOfString;
  NewLines: TArrayOfString;
  I: Integer;
  FoundConnectionNode: Boolean;
  NewConnectionLine: string;
  InsideConnectionNode: Boolean;
  LineCount: Integer;
begin
  if CurStep = ssPostInstall then
  begin
    // Obtener los valores ingresados por el usuario
    OracleHost := EditServer.Text;
    OraclePort := EditPort.Text;
    OracleServiceName := EditServiceName.Text;
    OracleUser := EditUser.Text;
    OraclePassword := EditPassword.Text;

    // Escapar caracteres especiales en XML usando nuestra función personalizada
    OraclePassword := EscapeXmlString(OraclePassword);
    OracleUser := EscapeXmlString(OracleUser);
    OracleHost := EscapeXmlString(OracleHost);
    OracleServiceName := EscapeXmlString(OracleServiceName);

    // CAMBIO PRINCIPAL: Usar formato EZ Connect en lugar de TNS completo
    ConnectionString := Format('Data Source=%s:%s/%s;User Id=%s;Password=%s;', [OracleHost, OraclePort, OracleServiceName, OracleUser, OraclePassword]);

    // Ruta al archivo de configuración (debe ser .exe.config para aplicaciones .NET)
    AppConfigFile := ExpandConstant('{app}\SisGestorEmpenio.dll.config');

    // Verificar si el archivo existe y leer líneas
    if FileExists(AppConfigFile) and LoadStringsFromFile(AppConfigFile, Lines) then
    begin
      FoundConnectionNode := False;
      InsideConnectionNode := False;
      LineCount := 0;
      SetArrayLength(NewLines, GetArrayLength(Lines));
      
      // Procesar cada línea
      for I := 0 to GetArrayLength(Lines) - 1 do
      begin
        // Buscar el inicio del nodo de conexión
        if Pos('<add name="MiConexionOracle"', Lines[I]) > 0 then
        begin
          InsideConnectionNode := True;
          FoundConnectionNode := True;
          
          // Agregar la nueva línea de conexión con formato EZ Connect
          NewConnectionLine := Format('    <add name="MiConexionOracle" connectionString="%s" providerName="Oracle.ManagedDataAccess.Client"/>', [ConnectionString]);
          NewLines[LineCount] := NewConnectionLine;
          LineCount := LineCount + 1;
          
          // Si la etiqueta se cierra en la misma línea, continuar con la siguiente línea
          if Pos('/>', Lines[I]) > 0 then
          begin
            InsideConnectionNode := False;
          end;
        end
        else if InsideConnectionNode then
        begin
          // Estamos dentro del nodo multi-línea, verificar si es la línea de cierre
          if Pos('/>', Lines[I]) > 0 then
          begin
            InsideConnectionNode := False;
            // No agregamos esta línea porque ya agregamos la línea completa arriba
          end;
          // Saltar todas las líneas dentro del nodo multi-línea
        end
        else
        begin
          // Línea normal, copiarla tal como está
          NewLines[LineCount] := Lines[I];
          LineCount := LineCount + 1;
        end;
      end;

      if FoundConnectionNode then
      begin
        // Ajustar el tamaño del array al número real de líneas
        SetArrayLength(NewLines, LineCount);
        
        // Crear una copia de seguridad antes de modificar
        if not FileCopy(AppConfigFile, AppConfigFile + '.backup', False) then
          Log('WARNING: No se pudo crear copia de seguridad del archivo de configuración');
        
        // Escribir las líneas actualizadas de vuelta al archivo con codificación UTF-8
        if SaveStringsToUTF8File(AppConfigFile, NewLines, False) then
        begin
          Log(Format('Cadena de conexión de Oracle actualizada en %s', [AppConfigFile]));
          Log(Format('Nueva cadena de conexión: %s', [ConnectionString]));
          
          // Validar que el archivo XML sea válido después de la modificación
          if not ValidateXmlFile(AppConfigFile) then
          begin
            Log('ERROR: El archivo de configuración generado no es XML válido');
            MsgBox('ERROR: El archivo de configuración generado contiene errores de formato XML.', mbError, MB_OK);
            
            // Restaurar copia de seguridad
            if FileExists(AppConfigFile + '.backup') then
              FileCopy(AppConfigFile + '.backup', AppConfigFile, False);
          end;
        end else
        begin
          Log(Format('ERROR: No se pudo escribir el archivo de configuración: %s', [AppConfigFile]));
          MsgBox('ERROR: No se pudo actualizar el archivo de configuración.', mbError, MB_OK);
          
          // Restaurar copia de seguridad si existe
          if FileExists(AppConfigFile + '.backup') then
            FileCopy(AppConfigFile + '.backup', AppConfigFile, False);
        end;
      end else
      begin
        Log(Format('ERROR: No se encontró la entrada de conexión "MiConexionOracle" en %s', [AppConfigFile]));
        MsgBox('ERROR: La cadena de conexión no se pudo actualizar. Revise la configuración manualmente.', mbError, MB_OK);
      end;
    end else
    begin
      Log(Format('ERROR: No se pudo leer el archivo de configuración: %s', [AppConfigFile]));
      MsgBox('ERROR: No se pudo leer el archivo de configuración.', mbError, MB_OK);
    end;
  end;
end;