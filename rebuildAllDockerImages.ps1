$Path = "C:\Work\MicroCouriers\src\"
docker build -t mcweb $Path'Web'
docker build -t bookingapi $Path'Services\Booking'
docker build -t paymentapi $Path'Services\Payment'
docker build -t trackingapi $Path'Services\Tracking'