from django.http import HttpResponse
from airbrake.utils.client import Client

# Create your views here.


def index(request):
    try:
        1/0
    except Exception as exception:
        airbrake = Client()
        airbrake.notify(exception, request)
    return HttpResponse("Hello, world. You're at the index.")