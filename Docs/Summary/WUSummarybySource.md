## Summary Resources by Source.
<span style="color:red">Requires authentication</span>  
Returns a water use summary resources by source.

Results returned by each response as shown in the following sample or by selecting the below load response button below;
#### Request
```
	$.ajax({
		url: url,
		success: function(resultData) { 
			var result = resultData;
		},
		error: function() {
			control.state('error');
		}
	});
```
