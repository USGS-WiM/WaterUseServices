## Category batch upload
<span style="color:red">Requires Administrators Authentication</span>   
Provides the ability to batch upload category resources.

#### Request Example
The REST URL section below displays the example url and the body/payload of the request used to simulate a response.

```
POST /wateruseservices/categories/batch HTTP/1.1
Host: streamstats.usgs.gov
Accept: application/json
content-type: application/json;charset=UTF-8
content-length: 576

[{
    "name":"CategorySample 1",
    "description":"Description of Category Sample 1",
    "code":"UniqueCode1"
},
{
    "name":"CategorySample 2",
    "description":"Description of Category Sample 2",
    "code":"UniqueCode2"
},
{
    "name":"CategorySample 3",
    "description":"Description of Category Sample 3",
    "code":"UniqueCode3"
}]
```

```
HTTP/1.1 200 OK
[{
	"id":51,
    "name":"CategorySample 1",
    "description":"Description of Category Sample 1",
    "code":"UniqueCode1"
},
{
	"id":52,
    "name":"CategorySample 2",
    "description":"Description of Category Sample 2",
    "code":"UniqueCode2"
},
{
	"id":53,
    "name":"CategorySample 3",
    "description":"Description of Category Sample 3",
    "code":"UniqueCode3"
}]
```