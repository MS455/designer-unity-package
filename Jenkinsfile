// Place this script under the root directory of your Unity Project and push to Git (SCM)

// Place your values for the below flags
def ADDRESSABLE_BUILD_ARCHIVE_STORAGE_ACCOUNT = "tulanedigcontent" // PUt the id of the credential of which storage account or container name to upload here.
def BUNDLE_ID = "com.hexaware.decore"
def IS_DEVELOPMENT_BUILD = true
def IS_DEBUGGING_MODE = true
def TEAMS_CHANNEL_WEBHOOK = "https://hexawareonline.webhook.office.com/webhookb2/b016bbab-9194-41d7-828c-b12fa4157757@7c0c36f5-af83-4c24-8844-9962e0163719/JenkinsCI/10b2b20a90bc43bfa694d6ed8c50632c/879613b8-9e16-4996-85a0-7fe738c46c38"
def ANDROID_KEYSTORE_NAME = "user.keystore"
def ANDROID_KEY_ALIAS_NAME = "com.test"

// Do not touch the values of the below variables
def UNITY_PATH
def UNITY_VERSION
def GITHUB_AUTHOR_NAME
def BRANCH_NAME = "development"
def DEVELOPMENT_BUILD_FLAG = ""
def DEBUGGING_FLAG = ""
def ADDRESSABLE_REMOTE_PATH = ""
def ADDRESSABLE_DEV_PROFILE = "dev"
def ADDRESSABLE_PROD_PROFILE = "prod"
def POSTMAN_COLLECTION
def BUILD_TYPE = "canary"
def JOB_NAME
def ASSEMBLY_NAME
def SONAR_CUBE_URL
def SONAR_CUBE_TOKEN
def SONARQUBE_ANALYSIS = "Failed/Did not reach Analysis step"
def TEST_CASE_ANALYSIS = "Failed/Did not reach unit tests step"
def RELEASE_NAME = "Failed/Did not reach release creation"
def LAST_STABLE_BUILD = "${env.RUN_DISPLAY_URL}"

pipeline {
    // Inside the parameters, the first choice for CHOICE is the first value of the array.
    parameters {
         choice(name: 'UNITY_VERSION', choices: ['2019.4.35f1', '2020.3.6f1', '2021.2.13f1'], description: 'Choose your Unity Engine Version')
         choice(name: 'TARGET_PLATFORM', choices: ['Android', 'iOS', 'Win64'], description: 'Choose your build target')
         string(name: 'BUNDLE_ID', defaultValue: "${BUNDLE_ID}", trim: true)
         booleanParam(name: 'IS_DEVELOPMENT_BUILD', defaultValue: "${IS_DEVELOPMENT_BUILD}", description: 'Is this a development build?')
         booleanParam(name: 'IS_DEBUGGING_MODE', defaultValue: "${IS_DEBUGGING_MODE}", description: 'Do you want to allow debugging?')
         string(name: 'TEAMS_CHANNEL_WEBHOOK', defaultValue: "${TEAMS_CHANNEL_WEBHOOK}", trim: true)
      
    }
    
    agent any
    options {
        timestamps()
        // as a failsafe. our build tend around the 15 mins mark, as 45 mins would be excessive.
        timeout(time: 5, unit: 'HOURS')
    }
    // post stages only kick in once we definitely have a node
    stages {
        stage('Preflight') {
            steps {
                
                checkout([$class: 'GitSCM', 
                    branches: [[name: '*/${BRANCH_NAME}']], 
                    doGenerateSubmoduleConfigurations: false, 
                    extensions: scm.extensions + [[$class: 'CleanCheckout'], [$class: 'GitLFSPull']],
                    submoduleCfg: [], 
                    userRemoteConfigs: scm.userRemoteConfigs
                ])
               bat 'dir'
               script {
                   JOB_NAME = env.JOB_NAME.toLowerCase().replace("/", "-").replace("_", "-")
                   ASSEMBLY_NAME = env.JOB_NAME.replace("/", "_")
                   echo "Fixed ${JOB_NAME}"
                   UNITY_PATH = "C:\\Program Files\\Unity\\Hub\\Editor\\${params.UNITY_VERSION}\\Editor\\Unity.exe"
                   BUILD_PATH = "${JENKINS_HOME}\\jobs\\${JOB_NAME}\\builds\\${BUILD_NUMBER}"
                   GITHUB_AUTHOR_NAME=bat(returnStdout: true, script: "git show -s --pretty=%%an").trim()
                   GITHUB_AUTHOR_NAME=GITHUB_AUTHOR_NAME.readLines().drop(1).join(" ")
                   if(params.IS_DEVELOPMENT_BUILD) {
                        DEVELOPMENT_BUILD_FLAG = "-optionDevelopment"
                        BUILD_TYPE = "dev"                        
                   }
                   if(params.IS_DEBUGGING_MODE) {
                        DEBUGGING_FLAG = "-optionAllowDebugging"
                        BUILD_TYPE = "dev"
                   }
               }
                
                
                echo "Commited user name: ${GITHUB_AUTHOR_NAME}"
                echo "BUILD PATH :${BUILD_PATH}"
                echo "Unity Installation Path : ${UNITY_PATH}"
                echo "DEVELOPMENT Build : ${IS_DEVELOPMENT_BUILD}"
            }
           
        }
        
        stage('API Health Tests') {
            
                when {
                    expression { 
                       return (findFiles(glob: "*.postman_collection.json").length > 0)
                    }
                }

                steps {
                    
                    echo "Health Checking for APIs"
                    script{
                        def filename = findFiles glob: '*.postman_collection.json'
                        POSTMAN_COLLECTION = filename[0]
                    }
                    
                    echo "Postman Collection filename : ${POSTMAN_COLLECTION}"
                    
                    bat "newman run ${POSTMAN_COLLECTION} --disable-unicode -r cli,junit --reporter-junit-export api-test-results.xml" 
                    junit 'api-test-results.xml'
                }
        }
        
//         stage('Generate Assemblies') {

//             options {
//                 timeout(time: 30, unit: "MINUTES")
//             }

// 			when {
//                     expression { 
//                        return true
//                     }
//                 }
//             steps {
//                 echo "Generating Solution .sln and .csproj files, and generating Unity Assemblies under /Library"
//                 bat "\"${UNITY_PATH}\" -batchmode -logFile output.log -quit -projectPath . -executeMethod UnityToolbag.SyncSolution.Sync"
//                 // bat "\"${UNITY_PATH}\" -batchmode -logFile output.log -quit -projectPath . -executeMethod UnityToolbag.GenerateSlnFiles.Sync"
//                 // bat "\"${UNITY_PATH}\" -batchmode -logFile output.log -quit -projectPath . -executeMethod UnityEditor.SyncVS.SyncSolution"
//                 sleep 30
//                 bat "dir"
//             }
//         }
        
//          stage('Code Quality Analysis') {
// 		 when {
//                     expression { 
//                        return true
//                     }
//                 }
				
//             environment {
//                 scannerHome = tool 'SonarQube Scanner for MS Build'
//             }
//             steps{
//                 echo "Running and syncing code base with SonarQube"
//                 bat 'dir'
//                 withSonarQubeEnv('sonarqube') {
//                     echo "Sonar Host Url: ${env.SONAR_HOST_URL}"
//                     echo "Blue Ocean Build URL: ${env.RUN_DISPLAY_URL}"
                    
//                     bat "${scannerHome}\\SonarScanner.MSBuild.exe begin /k:${JOB_NAME}"
//                     bat "MSBuild.exe ${ASSEMBLY_NAME}.sln /t:Rebuild" 
//                     bat "${scannerHome}\\SonarScanner.MSBuild.exe end"
//                    script{
//                         SONAR_CUBE_URL = env.SONAR_HOST_URL
//                         echo "$SONAR_CUBE_URL"
//                         SONAR_CUBE_TOKEN = env.SONAR_AUTH_TOKEN
                        
//                         final String rawSonarQubeUri = env.SONAR_HOST_URL + "/api/measures/component_tree?component="+JOB_NAME+"&metricKeys=bugs,vulnerabilities,code_smells,security_hotspots"
//                         final String sonarQubeTempResponse = bat(script: "curl -u $env.SONAR_AUTH_TOKEN: \"$rawSonarQubeUri\"", returnStdout: true).trim()
//                         final String sonarQubeResponse = sonarQubeTempResponse.readLines().drop(1).join(" ")
//                         echo sonarQubeResponse
//                         def json = new groovy.json.JsonSlurperClassic().parseText(sonarQubeResponse)

//                         echo "$json.baseComponent.measures" 
//                         def codeSmellsValue = ""
//                         def bugsValue = ""
//                         def vulnerabilitiesValue = ""
//                         def securityHotspotsValue = ""
//                         for (int i = 0; i < json.baseComponent.measures.size(); i++) {
//                             if (json.baseComponent.measures[i].metric.equals("code_smells")) {
//                                     codeSmellsValue = json.baseComponent.measures[i].value
//                             }
//                             if (json.baseComponent.measures[i].metric.equals("bugs")) {
//                                     bugsValue = json.baseComponent.measures[i].value
//                             }
//                             if (json.baseComponent.measures[i].metric.equals("vulnerabilities")) {
//                                     vulnerabilitiesValue = json.baseComponent.measures[i].value
//                             }
//                             if (json.baseComponent.measures[i].metric.equals("security_hotspots")) {
//                                     securityHotspotsValue = json.baseComponent.measures[i].value
//                             }
//                         }
//                     SONARQUBE_ANALYSIS = "$codeSmellsValue Code Smells | $bugsValue Bugs | $vulnerabilitiesValue Vulnerabilities | $securityHotspotsValue Security Hotspots"
//                    }
//                 }
//                 script{
//                 def qualitygate = waitForQualityGate()
//                  echo "Quality Gate Status : ${qualitygate}"
//                   if (qualitygate.status != "OK") {
//                      error "Pipeline aborted due to quality gate coverage failure: ${qualitygate.status}"
//                   }}
             
//             }
//         }

        stage('Build Addressables'){
            
            //AddressableAssetsData -> Check if this folder exist
            when {
                    expression { 
                       return (fileExists("Assets/AddressableAssetsData") == true)
                    }
                }
            
            steps{
                // What happens when addressables are not setup?????
                // If it doesnt, skip this step
                echo "Building Addressables ..."
                echo "Current workspace is ${env.WORKSPACE}" 
                echo "Profile Name : ${params.ADDRESSABLE_PROFILE}"
                
                script {
                    if(params.IS_DEVELOPMENT_BUILD || params.IS_DEBUGGING_MODE){
                        echo "Development/Debugging Build detected, switching to Dev Addressable Profile"
                        bat "\"${UNITY_PATH}\" -nographics -batchmode -quit -executeMethod BuildPlayerCommand.BuildAddressableWithProfiles -silent-crashes -buildTarget \"${params.TARGET_PLATFORM}\" -addressableprofile ${ADDRESSABLE_DEV_PROFILE}"
    
                    }
                    else{
                        echo "NON Development/Debugging Build detected, switching to Prod Addressable Profile"
                        bat "\"${UNITY_PATH}\" -nographics -batchmode -quit -executeMethod BuildPlayerCommand.BuildAddressableWithProfiles -silent-crashes -buildTarget \"${params.TARGET_PLATFORM}\" -addressableprofile ${ADDRESSABLE_PROD_PROFILE}"
                    }
                    
                } 
                
                sleep 30 
            }
        }

        stage('Archiving Addressables to Azure Blob Storage') {
            when {
                expression {
                    return (fileExists("ServerData") == true)
                }
            }
            steps {
                script {
                    withCredentials([string(credentialsId: 'tulanedigcontent', variable: 'ADDRESSABLE_STORAGE_ACCOUNT_KEY')]) { //set SECRET with the credential content
                        final String containerStatusTempResponse = bat(script: "az storage container exists --account-name $ADDRESSABLE_BUILD_ARCHIVE_STORAGE_ACCOUNT --account-key $ADDRESSABLE_STORAGE_ACCOUNT_KEY --name ${JOB_NAME}-addressable", returnStdout: true).trim()
                        final String containerStatusResponse = containerStatusTempResponse.readLines().drop(1).join(" ")
                        echo "Container Exists: $containerStatusResponse"
                        def containerStatusJson = new groovy.json.JsonSlurperClassic().parseText(containerStatusResponse)
                        if(containerStatusJson.exists == true) {
                            final String containerDeleteTempResponse = bat(script: "az storage blob delete-batch --account-name $ADDRESSABLE_BUILD_ARCHIVE_STORAGE_ACCOUNT --account-key $ADDRESSABLE_STORAGE_ACCOUNT_KEY -s ${JOB_NAME}-addressable --pattern $params.TARGET_PLATFORM/*", returnStdout: true).trim()
                            final String containerDeleteResponse = containerDeleteTempResponse.readLines().drop(1).join(" ")
                            echo "Container Deletion Status: $containerDeleteResponse"
                   
                        }
                        else {
                            final String containerCreateTempResponse = bat(script: "az storage container create --account-name $ADDRESSABLE_BUILD_ARCHIVE_STORAGE_ACCOUNT --account-key $ADDRESSABLE_STORAGE_ACCOUNT_KEY -n ${JOB_NAME}-addressable --public-access blob", returnStdout: true).trim()
                            final String containerCreateResponse = containerCreateTempResponse.readLines().drop(1).join(" ")
                            echo "Container Creation Status: $containerCreateResponse"
      
                        }
                        
                        final String containerUploadTempResponse = bat(script: "az storage blob upload-batch -d ${JOB_NAME}-addressable --account-name $ADDRESSABLE_BUILD_ARCHIVE_STORAGE_ACCOUNT --account-key $ADDRESSABLE_STORAGE_ACCOUNT_KEY -s ServerData", returnStdout: true).trim()
                        final String containerUploadResponse = containerUploadTempResponse.readLines().drop(1).join(" ")
                        echo "Addressables Upload Status: $containerUploadResponse"

                    }
                }
            }
        }
        
        stage('Editor Mode Unit Tests') {
            steps{
                echo "Running Unity in Editor"
                bat "\"${UNITY_PATH}\" -nographics -batchmode -projectPath . -runTests -testResults editmodetests.xml -silent-crashes -testPlatform editmode -logFile"
                sleep 30
                nunit testResultsPattern: 'editmodetests.xml'
            }
        }
        
         stage('Play Mode Unit Tests') {
            steps{
                echo "Running Unity in Play Mode"
                bat "\"${UNITY_PATH}\" -batchmode -projectPath . -runTests -testResults playmodetests.xml -silent-crashes -testPlatform playmode -logFile"
                sleep 30
                nunit testResultsPattern: 'playmodetests.xml'
                script {
                      def build = manager.build
                      def testFailResults = build.getAction(hudson.tasks.junit.TestResultAction.class).getFailCount();
                      def testTotalResults = build.getAction(hudson.tasks.junit.TestResultAction.class).getTotalCount();
                      PASSED_TESTCASES_COUNT = testTotalResults - testFailResults
                      echo "PASSED TEST CASES COUNT: $PASSED_TESTCASES_COUNT"
                      FAILED_TESTCASES_COUNT = testFailResults
                      echo "FAILED TEST CASES COUNT: $FAILED_TESTCASES_COUNT"
                      TEST_CASE_ANALYSIS = "${PASSED_TESTCASES_COUNT} Passed | ${FAILED_TESTCASES_COUNT} Failed" 
                }
            }
        }
        
        stage('Create Release'){
            steps {
                   echo "Current workspace is ${env.WORKSPACE}"
                   withCredentials([string(credentialsId: 'unity-ci-cd-pipeline-android-key-alias-password', variable: 'ANDROID_KEY_ALIAS_PASSWORD'), string(credentialsId: 'unity-ci-cd-pipeline-android-key-store-password', variable: 'ANDROID_KEY_STORE_PASSWORD')]) { //set SECRET with the credential content
                        bat "\"${UNITY_PATH}\" -nographics -batchmode -quit -executeMethod BuildPlayerCommand.Execute -buildName \"${JOB_NAME}\" ${DEVELOPMENT_BUILD_FLAG} ${DEBUGGING_FLAG} -silent-crashes -buildTarget \"${params.TARGET_PLATFORM}\" -applicationIdentifier \"${params.BUNDLE_ID}\" -bundleVersion \"${BUILD_NUMBER}\" -androidKeyAliasName \"${ANDROID_KEY_ALIAS_NAME}\" -androidKeyAliasPass \"$ANDROID_KEY_ALIAS_PASSWORD\" -androidKeyStoreName \"${ANDROID_KEYSTORE_NAME}\" -androidKeyStorePass \"$ANDROID_KEY_STORE_PASSWORD\" -projectPath \"${env.WORKSPACE}\" -outputDestination \"${JENKINS_HOME}/jobs/${JOB_NAME}/builds/${BUILD_NUMBER}/output\" -stripingDefineSymbols Test -logFile"
                   }
                   sleep 30
                   bat "if exist Temp (rmdir Temp /s /q)"
                   bat "if exist *.csproj (del *.csproj)"
                   bat "if exist *.sln (del *.sln)"
                   bat "if exist ServerData (rmdir ServerData /s /q)"
            }            
        }
   
        stage('Archive Artifacts'){
            steps {
                    echo "Zipping and Pushing to Azure Blob Storage"
                    script{
                        RELEASE_NAME = "release-${JOB_NAME}-${BUILD_NUMBER}-${TARGET_PLATFORM}-${BUILD_TYPE}.zip"
                        LAST_STABLE_BUILD = "${env.JOB_URL}lastSuccessfulBuild/artifact/${RELEASE_NAME}"
                    }
                    dir("${BUILD_PATH}") 
                    {
                        echo "Generating release zip file"
                        
                        bat "tar -a -c -f ${RELEASE_NAME} output"
                        echo "Uploading the zip to blob"
                        archiveArtifacts "*.zip"
                        echo "Deleting the zip file from local directory"
                        bat "if exist \"${BUILD_PATH}/*.zip\" (del *.zip)"    
                        bat "if exist \"${BUILD_PATH}/output\" (del output /s /q)" 
                    }
            }            
        }
    }
    
    post {
        success {
                echo 'Sending Success Notification'
                bat "curl --header \"Content-Type:application/json\" --request POST --data \"{'@type':'MessageCard','@context':'https://schema.org/extensions','summary':'Card Test card','themeColor':'00ff00','title':'${JOB_NAME}','sections':[{'activityTitle':'Build Information of #${BUILD_NUMBER}','facts':[{'name':'Status','value':'${currentBuild.currentResult}'},{'name':'Target OS','value':'${TARGET_PLATFORM}'},{'name':'Started by','value':'${GITHUB_AUTHOR_NAME}'},{'name':'Tests','value':'${TEST_CASE_ANALYSIS}'},{'name':'Code Quality','value':'${SONARQUBE_ANALYSIS}'},{'name':'Artifact','value':'${RELEASE_NAME}'}]}],'potentialAction':[{'@type':'OpenUri','name':'View Job','targets':[{'os':'default','uri':'${env.RUN_DISPLAY_URL}'}]},{'@type':'OpenUri','name':'View Logs','targets':[{'os':'default','uri':'${BUILD_URL}consoleText'}]},{'@type':'OpenUri','name':'View Code Quality','targets':[{'os':'default','uri':'${SONAR_CUBE_URL}/dashboard?id=${JOB_NAME}'}]},{'@type':'OpenUri','name':'Last Stable Release','targets':[{'os':'default','uri':'${LAST_STABLE_BUILD}'}]}]}\" ${TEAMS_CHANNEL_WEBHOOK}"    
        }
        failure {
                echo 'Sending FailureNotification'
                bat "curl --header \"Content-Type:application/json\" --request POST --data \"{'@type':'MessageCard','@context':'https://schema.org/extensions','summary':'Card Test card','themeColor':'ff0000','title':'${JOB_NAME}','sections':[{'activityTitle':'Build Information of #${BUILD_NUMBER}','facts':[{'name':'Status','value':'${currentBuild.currentResult}'},{'name':'Target OS','value':'${TARGET_PLATFORM}'},{'name':'Started by','value':'${GITHUB_AUTHOR_NAME}'},{'name':'Tests','value':'${TEST_CASE_ANALYSIS}'},{'name':'Code Quality','value':'${SONARQUBE_ANALYSIS}'},{'name':'Artifact','value':'${RELEASE_NAME}'}]}],'potentialAction':[{'@type':'OpenUri','name':'View Job','targets':[{'os':'default','uri':'${env.RUN_DISPLAY_URL}'}]},{'@type':'OpenUri','name':'View Logs','targets':[{'os':'default','uri':'${BUILD_URL}consoleText'}]},{'@type':'OpenUri','name':'View Code Quality','targets':[{'os':'default','uri':'${SONAR_CUBE_URL}/dashboard?id=${JOB_NAME}'}]},{'@type':'OpenUri','name':'Last Stable Release','targets':[{'os':'default','uri':'${LAST_STABLE_BUILD}'}]}]}\" ${TEAMS_CHANNEL_WEBHOOK}"    
        }
        aborted {
                echo 'Sending Aborted Notification'
                bat "curl --header \"Content-Type:application/json\" --request POST --data \"{'@type':'MessageCard','@context':'https://schema.org/extensions','summary':'Card Test card','themeColor':'808080','title':'${JOB_NAME}','sections':[{'activityTitle':'Build Information of #${BUILD_NUMBER}','facts':[{'name':'Status','value':'${currentBuild.currentResult}'},{'name':'Target OS','value':'${TARGET_PLATFORM}'},{'name':'Started by','value':'${GITHUB_AUTHOR_NAME}'},{'name':'Tests','value':'${TEST_CASE_ANALYSIS}'},{'name':'Code Quality','value':'${SONARQUBE_ANALYSIS}'},{'name':'Artifact','value':'${RELEASE_NAME}'}]}],'potentialAction':[{'@type':'OpenUri','name':'View Job','targets':[{'os':'default','uri':'${env.RUN_DISPLAY_URL}'}]},{'@type':'OpenUri','name':'View Logs','targets':[{'os':'default','uri':'${BUILD_URL}consoleText'}]},{'@type':'OpenUri','name':'View Code Quality','targets':[{'os':'default','uri':'${SONAR_CUBE_URL}/dashboard?id=${JOB_NAME}'}]},{'@type':'OpenUri','name':'Last Stable Release','targets':[{'os':'default','uri':'${LAST_STABLE_BUILD}'}]}]}\" ${TEAMS_CHANNEL_WEBHOOK}"    
        }
    }
}
