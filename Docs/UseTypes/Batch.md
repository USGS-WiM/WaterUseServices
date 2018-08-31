## Use Type Batch Upload
<span style="color:red">Requires Administrators Authentication</span>  
Provides the ability to batch upload use type resources.


#### Request Example
The REST URL section below displays the example url and the body/payload of the request used to simulate a response.

```
POST /wateruseservices/usetypes/batch HTTP/1.1
Host: streamstats.usgs.gov
Accept: application/json
content-type: application/json;charset=UTF-8
content-length: 576

[{
    "name":"use type Sample 1",
    "code":"Ut1"
	"description":"use type sample 1"
},
{
    "name":"use type Sample 2",
    "code":"Ut2",
	"description":"use type sample 2"
}]
```

```
HTTP/1.1 200 OK
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
}]
```