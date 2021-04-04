# PillarTech-Kata
Checkout Order Total Kata - Done in C#
 
<br/><br/>

## Building/Running/Testing

Prior requirements: Docker or VS 2019 with .NET Desktop Development Workload
1. First, clone the repository to a path of your choosing

### VS 2019 Users:
Simply open the solution in VS and build either project. You can use the test explorer to run the tests for the solution

### Docker users:
1. Open the command line (PowerShell, CMD, etc...)
2. cd into the directory of the repository with the Dockerfile
3. Type ``` docker build -f "Dockerfile" -t "pillartechkata:latest" . ``` and hit enter to build the docker image which builds the solution
4. Type ``` docker run pillartechkata:latest ``` and hit enter to run the tests
