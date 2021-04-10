# dragonmite-server
This is beta server, for DML Dragonmite project
Technologies:
    - Programming language: Scala & Java 
    - Framework: Akka Toolkit
    - Build: sbt 
For Java developer, read 4. For Java dev only first.

1. Requires to run:
    - Scala, Java newest
    - Sbt newest
    - VsCode
    - Metal( VsCode extension, please install after install VSCode) 
2. How to config:
    - Open project in VSCode
    - Wait a second, Metal will alter need Import build, click on Import build 
    (If not, open Metal extension icon at left panel, click Import build)
    - Wait all library added
3. Run
    - Make sure your direction is: .../dragonmite/Dragonmite_server/beta_server and step 2 finished
    - Open or command line: terminal, cmd or powershell
    - Run cmd: sbt run
    (Wait a lot when sbt compile all code)

    - Choose your server to start by enter number before server name(In this case are 1 & 2):        
        Multiple main classes detected. Select one to run:
        [1] DragonMiteServer_ServerEntryPoint_InScala --> Scala Server
        [2] DragonmiteServer.ServerEntryPoint         --> Java Server

    - Server entry point for testing: wss://localhost:8002 
    (You can easy edit in ServerEntryPoint.scala or ServerEntryPoint.java)

4. For Java developer 
    - Maintain and add new code in src/main/java and src/test/java
    - Add dependencies from https://mvnrepository.com/, choose sbt, put in build.sbt file
    - To run, follow step 3.
    - If have any error, report me via email: quynh.fullstackdev@gmail.com. 
    
Best regards !
Nguyen Manh Quynh.
