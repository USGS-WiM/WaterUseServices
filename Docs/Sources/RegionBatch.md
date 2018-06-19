## Region batch upload
<span style="color:red">Requires Authentication</span>  
Provides the ability to batch upload region source resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfsources = 
[
	{
	"name": "test source",
	"facilityName": "WELL test # 3,
	"facilityCode": "abc345",
	"stationID": "",
	"catagoryTypeID": 1,
	"sourceTypeID": 1,
	"useTypeID": 1,
	"location": {
		"x": -112.2345,
		"y": 42.4566,
		"srid": 4269
		}
	},
  {
	"name": "test source 2",
	"facilityName": "WELL test #4 ",
	"facilityCode": "abc123",
	"stationID": "",
	"catagoryTypeID": 1,
	"sourceTypeID": 1,
	"useTypeID": 1,
	"location": {
		"x": -112.2789,
		"y": 42.123,
		"srid": 4269
		}
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
Response result will return the List of region sources, with corresponding ID's. Similar to following example:

```
[
	{
	"id":1,
	"name": "test source",
	"facilityName": "WELL test # 3,
	"facilityCode": "abc345",
	"stationID": "",
	"catagoryTypeID": 1,
	"sourceTypeID": 1,
	"useTypeID": 1,
	"location": {
		"x": -112.2345,
		"y": 42.4566,
		"srid": 4269
		}
	},
  {
	"id":1,
	"name": "test source 2",
	"facilityName": "WELL test #4 ",
	"facilityCode": "abc123",
	"stationID": "",
	"catagoryTypeID": 1,
	"sourceTypeID": 1,
	"useTypeID": 1,
	"location": {
		"x": -112.2789,
		"y": 42.123,
		"srid": 4269
		}
	}
];
```