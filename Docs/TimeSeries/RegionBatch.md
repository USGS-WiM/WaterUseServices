## Time series batch upload
<span style="color:red">Requires authentication</span>  
Provides the ability to batch upload time series resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfTimeSeries = 
[{
    "facilityCode":"UniqueFacilityCode1",
    "date":"2010-04-01T00:00:00"
    "value":0.0036,
	"unitTypeID":1
},
{
    "facilityCode":"UniqueFacilityCode1
    "date":"2010-05-01T00:00:00"
    "value":0.0156,
	"unitTypeID":1
},
{
    "facilityCode":"UniqueFacilityCode2",
    "date":"2010-04-01T00:00:00"
    "value":0.0025
	"unitTypeID":1
}];

$.ajax({
        type: 'POST',
        url: url,
        crossDomain: true,
        data: JSON.stringify(listOfTimeSeries),
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
Response result will return the List of time series with corresponding ID's. Similar to following example:

```
[{
	"id": 25,
	"sourceID": 1,
	"date": "2010-04-01T00:00:00",
	"value": 0.0036,
	"unitTypeID": 1
},
{
	"id":52,
	"sourceID": 1,
	"date": "2010-05-01T00:00:00",
	"value": 0.0156
	"unitTypeID": 1
},
{
	"id":53,
    "sourceID": 2
	"date": "2010-04-01T00:00:00",
	"value": 0.0025
	"unitTypeID": 1
}]
```