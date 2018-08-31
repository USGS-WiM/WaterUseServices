## Region batch upload
<span style="color:red">Requires Administrators Authentication</span>    
Provides the ability to batch upload role resources.

Response as shown in the following sample.
#### Request Example
The REST URL section below displays the example url and the body/payload of the request used to simulate a response.

```
POST /wateruseservices/roles/batch HTTP/1.1
Host: streamstats.usgs.gov
Accept: application/json
content-type: application/json;charset=UTF-8
content-length: 176

[
	{
    "name":"testRole1",
    "description":"Description of role1"
	},
	{
    "name":"testRole2",
    "description":"Description of role2"
	}
]
```

```
HTTP/1.1 200 OK
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