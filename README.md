# Json2CSharpConverter
Json2CSharpConverter


Just an application that convert a json to C# code.

For example:
```json
{
	"creditInfo": {
		"account": "4230677804",
		"status": "ΕΝΕΡΓΟΣ             ",
		"product": {
			"code": "01414101Β01",
			"description": "Description"
		},
		"accountType": "ΚΑΤΑΝΑΛΩΤΙΚΟ        ",
		"accountOverdueStatus": "ΕΝΗΜΕΡΟ",
		"branch": "0040"
	}
}
```
To :
```csharp
{
	CreditInfo = new CreditInfo
	{
		Account = "4230677804",
		Status = "ΕΝΕΡΓΟΣ             ",
		Product = new Product
		{
			Code = "01414101Β01",
			Description = "Description"
		},
		AccountType = "ΚΑΤΑΝΑΛΩΤΙΚΟ        ",
		AccountOverdueStatus = "ΕΝΗΜΕΡΟ",
		Branch = "0040"
	}
}
```