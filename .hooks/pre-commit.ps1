$files = $(git diff --cached --name-only --diff-filter=ACM "*.csproj" "*.cs" "*.razor" "*.html" "*.css" "*.js" "*.json")
Write-Host "Files added, copied or modified: ${files}"

if ($files.Count -eq 0) {
    Write-Host "Exiting; no files added, copied or modified"
    exit 0
}

$joinedFiles = [string]::Join(';', $files)
jb cleanupcode Couple.sln --include=$joinedFiles

foreach ($file in $files) {
    git add $file
}
