## Login
<span style="color:red">Requires authentication</span>  
Request authenticated users configuration.

Response as shown in the following sample.
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
Response result will return the authenticated user, such as:

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