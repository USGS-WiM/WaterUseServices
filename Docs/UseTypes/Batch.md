## Use Type Batch Upload
<span style="color:red">Requires Administrators Authentication</span>  
Provides the ability to batch upload use type resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfusetypes = 
[{
    "name":"use type Sample 1",
    "code":"Ut1"
	"description":"use type sample 1"
},
{
    "name":"use type Sample 2",
    "code":"Ut2",
	"description":"use type sample 2"
}];

$.ajax({
        type: 'POST',
        url: url,
        crossDomain: true,
        data: JSON.stringify(listOfusetypes),
        dataType: 'json',
        contentType: 'application/json; charset=UTF-8',
        success: function(resultData) { 
            var results = resultData;
        },
        error: function() {
            control.state('error');
        }
    });
```
Response result will return the List of use types, with corresponding ID's. Similar to following example:

```
[{
	"id":1,
    "name":"use type Sample 1",
    "code":"Ut1"
	"description":"use type sample 1"
},
{
	"id":2,
    "name":"use type Sample 2",
    "code":"Ut2",
	"description":"use type sample 2"
}];
```