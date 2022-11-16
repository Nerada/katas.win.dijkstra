param($officialBuild)

$branch = git symbolic-ref --short -q HEAD
$gitRef = git describe --always --abbrev=6 --dirty --exclude '*'
$buildTime = [System.DateTimeOffset]::Now.ToString("o")

if($officialBuild -ne "official-build"){$user = $env:UserName + "|"}

$user + $branch + "|" + $gitRef + "|" + $buildTime