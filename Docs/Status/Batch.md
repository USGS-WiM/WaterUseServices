## Status batch upload
<span style="color:red">** **Administrators only** **  
Requires authentication</span>  
Provides the ability to batch upload Status resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfStatus = 
[{
    "name":"StatusSample 1",
    "description":"Description of Status Sample 1",
    "code":"UniqueCode1"
},
{
    "name":"StatusSample 2",
    "description":"Description of Status Sample 2",
    "code":"UniqueCode2"
},
{
    "name":"StatusSample 3",
    "description":"Description of Status Sample 3",
    "code":"UniqueCode3"
}];

$.ajax({
        type: 'POST',
        url: url,
        crossDomain: true,
        data: JSON.stringify(listOfStatus),
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
Response result will return the List of categories, with corresponding ID's. Similar to following example:

```
[{
	"id":51,
    "name":"Status Sample 1",
    "description":"Description of Status Sample 1",
    "code":"UniqueCode1"
},
{
	"id":52,
    "name":"Status Sample 2",
    "description":"Description of Status Sample 2",
    "code":"UniqueCode2"
},
{
	"id":53,
    "name":"Status Sample 3",
    "description":"Description of Status Sample 3",
    "code":"UniqueCode3"
}]
```