## Available Roles Resources
<span style="color:red">Requires authentication</span> 

Returns an array of available role resources currently provided by the services

Results returned by each response as shown in the following sample
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
    "name":"testRole",
    "description":"Description of role"
}]
```
