# Challenge
Challenge C#

Si al ejecutar la solución aparece el error: "No se puede encontrar una parte de la ruta de acceso '~\bin\roslyn\csc.exe'."

Ejecutar el siguiente comando desde el Package Manager:
PM > update-package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r

Y volver a compilar la solución
