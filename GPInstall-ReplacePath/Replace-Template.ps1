$replaceExePath = ".\bin\debug\net6.0\GPInstall-ReplacePath.exe"

function Replace-File($source, $destination)
{
	if($source -ne $destination)
	{cp $source $destination}
	& $replaceExePath $destination "\\Users\Install_Source\HRM_2008\ManualsLib" "R:\ManualsLib" $destination
	& $replaceExePath $destination "\\users\Install_Source\HRM_2008\" "R:\install\" $destination
}

Replace-File "R:\install\GPI\HRM_2008_pw.gpi" "R:\install\GPI\HRM_2008_pw.gpi"
Replace-File "R:\install\GPI\HRM_2008.gpi" "R:\install\GPI\HRM_2008.gpi"
Replace-File "R:\install\GPI\HRM_2008_upgr.gpi" "R:\install\GPI\HRM_2008_upgr.gpi"
Replace-File "R:\install\GPI\HRM_2008_upgr_no_instr.gpi" "R:\install\GPI\HRM_2008_upgr_no_instr.gpi"
