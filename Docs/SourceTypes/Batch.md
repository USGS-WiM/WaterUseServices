## Source Type batch upload
<span style="color:red">** **Administrators only** **  
Requires authentication</span>  
Provides the ability to batch upload Source type resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfSourceTypes = 
[{
    "name":"Source typeSample 1",
    "description":"Description of source type Sample 1",
    "code":"UniqueCode1"
},
{
    "name":"Source typeSample 2",
    "description":"Description of source type Sample 2",
    "code":"UniqueCode2"
},
{
    "name":"Source typeSample 3",
    "description":"Description of source type Sample 3",
    "code":"UniqueCode3"
}];

$.ajax({
        type: 'POST',
        url: url,
        crossDomain: true,
        data: JSON.stringify(listOfSourceTypes),
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
    "name":"Source type Sample 1",
    "description":"Description of Source type Sample 1",
    "code":"UniqueCode1"
},
{
	"id":52,
    "name":"Source type Sample 2",
    "description":"Description of Source type Sample 2",
    "code":"UniqueCode2"
},
{
	"id":53,
    "name":"Source type Sample 3",
    "description":"Description of Source type Sample 3",
    "code":"UniqueCode3"
}]
```