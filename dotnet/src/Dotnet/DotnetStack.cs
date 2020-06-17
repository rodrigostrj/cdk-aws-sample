using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ECS.Patterns;
using System.ComponentModel;

namespace Dotnet
{
    public class DotnetStack : Stack
    {
        internal DotnetStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var vpc = new Vpc(this, "MyVpc", new VpcProps
            {
                MaxAzs = 3 // Max zones
            });

            var cluster = new Cluster(this, "MyCluster", new ClusterProps
            {
                Vpc = vpc
            });

            var serviceProps = new ApplicationLoadBalancedFargateServiceProps()
            {
                Cluster = cluster,  // Required
                DesiredCount = 5,   // Default is 1
                TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                {
                    Image = ContainerImage.FromRegistry("amazon/amazon-ecs-sample") // PHP Sample :-( my bad => https://hub.docker.com/r/amazon/amazon-ecs-sample
                },
                MemoryLimitMiB = 2048, // Default is 256
                PublicLoadBalancer = true, // Default is false
            };

            // Create Loadbalancer fargate and make it public

            new ApplicationLoadBalancedFargateService(this, "MyFargateService",
                serviceProps);
        }
    }
}
