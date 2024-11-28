## ������, QualityPoint Development!

<details>
	<summary>������ ������� </summary>
		

	�������� ������: 
	���������� ����������� ���-������, ������� �������� �� ������������ �����, �������� � ������� DaData (https://dadata.ru/api/clean/address/) ��������� ��� ��������������, ����� ���������� ���������� ����� ������������ � ���� ������ � ������������ ������� �����. �������������� ������� � ���� ������� ��������� ����� �� ����� / ���� (���), ����� ���������� ������, ����������, ����� ������ � ������ �������� ����������. 

	���������� � ����������:
	-  ASP.NET Core WebApi 
	- 1 GET ��������, ��������� ������ ���� ������, ���������� ������, ���������� � ���� ������, �����, �����, ����� ����, ����� ��������� � �.�. � �� ���� ����������, �� �� ������ 5 �����
	- ������� ������������ DI ��� ���� ����������� ��������
	- ������������� Automapper ��� ����������� ��������/�������
	- ������ � ������� ������������ (��� ������, �������, ������ ������ ���� � appsettings.json, ������������ ������ IOptions)
	- ���������� ������ � HttpClient (������������� IHttpClientFactory ��� ��������������/����������� ��������)
	- ��������� ���������� � ������� middleware/��������/IExceptionHandler (�� ������ try/catch � �����������)
	- ����������� � ������� Serilog/NLog
	- ����������� CORS ��������
	- ������������ Swagger
	- ������ ������, ����� � �������� � �����

</details>

������ ������������ ����� ������� ���-������, ������������� � Dadata � ����������������� �����.

1. ������������� ����������:
	-  AutoMapper,
	- Serilog,
	- Swashbuckle,
	- Dadata,
	- Newtonsoft.Json

1. ��������� ������ � ������� middleware, ���������� ���������� � ������������ � ���� json-�������. ���� ��������� � ������� � � ���� (��������� ����� **Middleware** + ��������� ����� log)
1. �������� � API Dadata �������� ������ **DaDataService**.  ������ �������� ������ � `IHttpClientFactory` ��� �������� HTTP ��������. ��� ������������ ������������ ������ �� `appsettings.json`, ������� ����������� ����� `IOptions<DaDataSettings>`. ����� ���������� � ����� Services ��� �������� � ����������� �����.
	
	����� `StandardizeAddressAsync` ��������� ������ � �������, ���������, ��� ����� �� ������. �� ������� HTTP ������ � ������� `IHttpClientFactory`, ������� ������������ ��� �������� ������� � DaData API. ������ ������������ � ����� `CleanClientAsync.Clean<Address>`, ������� ��������� ����������������� �����. � ������ ��������� ������, ����� ���������� ����������������� �����. � ������ ������ ��� ���� API ������ ������ ���������, ����� �������� ������ � ����������� ����������.
4. ���������� AddressController, ��������� ������� �� �������������� �������. ����� ���������� ��� ������ � ��������� �������, ����� ���� ���������� ������ DaDataService ��� ��������������. ��������� �������������� ����������� � ������� AutoMapper � ������ ������ AddressResponse.