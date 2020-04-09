# Elastic APM POC

Just a simple implementation of Elastic APM with 2 API's in .NET Core and 2 Databases (Sql Server and RavenDb)


---

## Usage

 Go to folder main folder and start the docker-compose or start the docker-compose from Visual Studio

Access Raven Url to accept EULA, the default Url is http://localhost:8080

![image](https://github.com/Baldini/ElasticAPM_POC/blob/master/images/raven.png?raw=true)

Access the Kibana Url to configure APM server, the default Url is http://localhost:5601

![image](https://github.com/Baldini/ElasticAPM_POC/blob/master/images/kibana_1.png?raw=true)

Check APM Server Status

![image](https://github.com/Baldini/ElasticAPM_POC/blob/master/images/kibana_2.png?raw=true)

Load Kibana Objects and Launch APM

![image](https://github.com/Baldini/ElasticAPM_POC/blob/master/images/kibana_3.png?raw=true)

Done, just make a request (both API's have swagger) and see the APM Magic

![image](https://github.com/Baldini/ElasticAPM_POC/blob/master/images/kibana_4.png?raw=true)
