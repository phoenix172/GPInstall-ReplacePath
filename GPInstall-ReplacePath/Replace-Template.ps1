$replaceExePath = ".\bin\debug\net6.0\GPInstall-ReplacePath.exe"
cp "R:\install\GPI\HRM_2008_pw.gpi" "R:\install\GPI\HRM_2008_pw2.gpi"
& $replaceExePath "R:\install\GPI\HRM_2008_pw2.gpi" "\\Users\Install_Source\HRM_2008\ManualsLib" "R:\ManualsLib" "R:\install\GPI\HRM_2008_pw2.gpi"
& $replaceExePath "R:\install\GPI\HRM_2008_pw2.gpi" "\\users\Install_Source\HRM_2008\" "R:\install\" "R:\install\GPI\HRM_2008_pw2.gpi"
