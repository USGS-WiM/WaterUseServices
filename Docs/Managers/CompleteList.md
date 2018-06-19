## Available Manager Resources
<span style="color:red">Requires Administrators Authentication</span>  

Returns an array of available manager resources currently provided by the services

Results returned by each response as shown in the following sample or by selecting the below load response button below;
#### Sample Request
```
	$.ajax({
		url: url,
		success: function(resultData) { 
			var result = resultData;
		},
		error: function() {
			control.state('error');
		}
	});
```
Response result will return the an array of manager resources, such as:

```
[{
	"id":1,
    "firstname":"testLogin",
    "lastname":"testLast",
    "username":"Unique&UserName",
	"email":"email@test.com",
	"roleID":2
}]
```
