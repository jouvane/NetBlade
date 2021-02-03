# change these three variables to suit your requirements
$baseDirectory = "D:\p\"
$a = "Movies"
$b = "new-name"

Get-ChildItem -Recurse $baseDirectory | 
        Where-Object {$_.PSIsContainer -and ($_.GetFiles().Count -eq 0)} | 
        ForEach-Object{[void](New-Item -Path $_.FullName -Name ".gitkeep" -ItemType File)}

# get all files
$files = Get-ChildItem $baseDirectory -File -Recurse
# get all the directories
$directorys = Get-ChildItem $baseDirectory -Directory -Recurse

# replace the contents of the files only if there is a match
foreach ($file in $files)
{
    $fileContent = Get-Content -Path $file.FullName

    if ($fileContent -match $a)
    {
        $newFileContent = $fileContent -replace $a, $b
        Set-Content -Path $file.FullName -Value $newFileContent
    }
}

# change the names of the files first then change the names of the directories

# iterate through the files and change their names
foreach ($file in $files)
{
    if ($file -match $a)
    {
        $newName = $file.Name -replace $a, $b
        Rename-Item -Path $file.FullName -NewName $newName
    }
}

# reverse the array of directories so we go deepest first
# this stops us renaming a parent directory then trying to rename a sub directory which will no longer exist
# e.g.
# we might have a directory structure "C:\Rename\Rename"
# the file array would be [ C:\Rename, C:\Rename\Rename ]
# without reversing we'd rename the first directory to "C:\NewName"
# the directory structure would now be "C:\NewName\Rename"
# we'd then try to rename C:\Rename\Rename which would fail
[array]::Reverse($directorys)

# iterate through the directories and change their names
foreach ($directory in $directorys)
{
    if ($directory -match $a)
    {
        $newName = $directory.Name -replace $a, $b
        Rename-Item -Path $directory.FullName -NewName $newName
    }
}