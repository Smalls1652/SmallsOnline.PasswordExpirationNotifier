[CmdletBinding()]
param()

$scriptRoot = $PSScriptRoot

$writeInfoSplat = @{
    "InformationAction" = "Continue";
}

# ---- Bootstrap ----

# Bootstrap CSS file paths
$bootstrapCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\css\bootstrap.min.css"
$bootstrapCssMapPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\css\bootstrap.min.css.map"
$bootstrapOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\assets\css\bootstrap\"

# Create output directory if it doesn't exist
if (!(Test-Path -Path $bootstrapOutPath))
{
    $null = New-Item -Path $bootstrapOutPath -ItemType "Directory"
}

# Remove any existing item in the directory
foreach ($fileItem in (Get-ChildItem -Path $bootstrapOutPath))
{
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$( $fileItem.Name )'"
    Remove-Item -Path $fileItem.FullName -Force
}

# Copy the files
Write-Information @writeInfoSplat -MessageData "`t| bootstrap.min.css-> $( $bootstrapOutPath )"
Copy-Item -Path $bootstrapCssPath -Destination $bootstrapOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| bootstrap.min.css.map-> $( $bootstrapOutPath )"
Copy-Item -Path $bootstrapCssMapPath -Destination $bootstrapOutPath -ErrorAction "Stop"

# ---- Bootstrap icons ----

# Bootstrap icons file paths
$bootstrapIconsCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap-icons\font\bootstrap-icons.css"
$bootstrapIconsFontDirPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap-icons\font\fonts\"
$bootstrapIconsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\assets\css\bootstrap-icons\"

# Create output directory if it doesn't exist
if (!(Test-Path -Path $bootstrapIconsOutPath))
{
    $null = New-Item -Path $bootstrapIconsOutPath -ItemType "Directory"
}

# Remove any existing item in the directory
foreach ($fileItem in (Get-ChildItem -Path $bootstrapIconsOutPath))
{
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$( $fileItem.Name )'"
    Remove-Item -Path $fileItem.FullName -Force -Recurse
}

# Copy the files
Write-Information @writeInfoSplat -MessageData "`t| bootstrap-icons.css-> $( $bootstrapIconsOutPath )"
Copy-Item -Path $bootstrapIconsCssPath -Destination $bootstrapIconsOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| fonts\-> $( $bootstrapIconsOutPath )"
Copy-Item -Path $bootstrapIconsFontDirPath -Destination $bootstrapIconsOutPath -Recurse -ErrorAction "Stop"

# ---- Bootstrap JS ----

# Bootstrap JS file paths
$bootstrapJsPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\js\bootstrap.bundle.min.js"
$bootstrapJsMapPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\js\bootstrap.bundle.min.js.map"
$bootstrapJsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\assets\js\bootstrap\"

# Create output directory if it doesn't exist
if (!(Test-Path -Path $bootstrapJsOutPath))
{
    $null = New-Item -Path $bootstrapJsOutPath -ItemType "Directory" -Force
}

# Remove any existing item in the directory
foreach ($fileItem in (Get-ChildItem -Path $bootstrapJsOutPath))
{
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$( $fileItem.Name )'"
    Remove-Item -Path $fileItem.FullName -Force
}

# Copy the files
Write-Information @writeInfoSplat -MessageData "`t| bootstrap.bundle.min.js-> $( $bootstrapJsOutPath )"
Copy-Item -Path $bootstrapJsPath -Destination $bootstrapJsOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| bootstrap.bundle.min.js.map-> $( $bootstrapJsOutPath )"
Copy-Item -Path $bootstrapJsMapPath -Destination $bootstrapJsOutPath -ErrorAction "Stop"