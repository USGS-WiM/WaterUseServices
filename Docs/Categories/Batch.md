## Category batch upload
<span style="color:red">Requires Administrators Authentication</span>   
Provides the ability to batch upload category resources.

Response as shown in the following sample.
#### Sample Request
```
	var listOfCategories = 
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
}];
```
Response result will return the List of categories, with corresponding ID's. Similar to following example:

```
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