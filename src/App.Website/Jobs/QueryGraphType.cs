using App.Website.Schema;
using GraphQL;

namespace App.Website.Jobs;

[GraphQLMetadata("Query")]
[GraphQLSchemaMetadataAttribute("")]
public class QueryGraphType
{
    public object Jobs()
    {
        return new object();
    }
}
