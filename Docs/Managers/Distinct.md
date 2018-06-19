## Available Manager Resource
<span style="color:red">Requires Authentication</span>  
Returns the selected manager resource.

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
Response result will return the manager resource, such as:

```
{
	"id":1,
    "firstname":"testLogin",
    "lastname":"testLast",
    "username":"Unique&UserName",
	"email":"email@test.com",
	"roleID":2
}
```