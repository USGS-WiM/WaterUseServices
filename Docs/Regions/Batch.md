## Region batch upload
<span style="color:red">Requires Administrators Authentication</span>    
Provides the ability to batch upload region resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfNewRegions = 
[
	{"name":"testRegion",
	"shortName":"AB",
	"description":"testRegion",
	"fipsCode":"55"
	},
	{"name":"testRegion2",
	"shortName":"CD",
	"description":"testRegion2",
	"fipsCode":"56"
	}
];

$.ajax({
        type: 'POST',
        url: url,
        crossDomain: true,
        data: JSON.stringify(listOfNewRegions),
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
[
	{
	"id":1,
	"name":"testRegion",
	"shortName":"AB",
	"description":"testRegion",
	"fipsCode":"55"
	},
	{
	"id":2,
	"name":"testRegion2",
	"shortName":"CD",
	"description":"testRegion2",
	"fipsCode":"56"
	}
]
```