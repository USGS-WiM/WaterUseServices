## Unit Batch Upload
<span style="color:red">** **Administrators only** **  
Requires authentication</span>  
Provides the ability to batch upload unit resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfUnits = 
[{
    "name":"unitSample 1",
    "abbreviation":"US1"
},
{
    "name":"unitSample 2",
    "abbreviation":"US2"
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
Response result will return the List of units, with corresponding ID's. Similar to following example:

```
[{
	"id":1,
    "name":"unitSample 1",
    "abbreviation":"US1"
},
{
	"id":2,
    "name":"unitSample 2",
    "abbreviation":"US2"
}]
```