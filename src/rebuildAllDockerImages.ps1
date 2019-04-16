docker build -t bookingapi -f BookingDockerfile .
docker build -t paymentapi -f PaymentDockerfile .
docker build -t trackingapi -f TrackingDockerfile .
docker build -t mcweb -f McWebDockerfile .

Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');