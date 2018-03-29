## Region batch upload
<span style="color:red">** **Administrators only** **  
Requires authentication</span>  
Provides the ability to batch upload role resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfNewRoles = 
[
	{
    "name":"testRole1",
    "description":"Description of role1"
	},
	{
    "name":"testRole2",
    "description":"Description of role2"
	}
];

$.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(listOfNewRoles),
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
Response result will return the List of Roles, with corresponding ID's. Similar to following example:

```
[
	{
	"id":1,
    "name":"testRole2",
    "description":"Description of role2"
	},
	{
	"id":2,
    "name":"testRole2",
    "description":"Description of role2"
	}
]
```