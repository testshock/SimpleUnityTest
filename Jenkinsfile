//@Library('jenkins-shared-library') _
node('NativeMacOSJenkins') {
    // Checkout code
    checkout scm

    // Load static properties from config.properties
    properties = readProperties file: 'config.properties'

    // Generate version file with static and dynamic info
    GIT_COMMIT_SHORT = sh(
       script: "git rev-parse --short=6 HEAD",
       returnStdout: true
    )
    GIT_COMMIT_SHORT = GIT_COMMIT_SHORT.replaceAll("\n","")

    createVersionFile(properties,GIT_COMMIT_SHORT)

    // Global Exception handling.
    try {
        echo "BUILD"

        // Build iOS Unity project IF key iOS='TRUE' (in config.properties file)

        if (properties.iOS == 'TRUE') {
            stageName='Build Unity iOS'
            stage(stageName){
                sh "/Applications/Unity/Hub/Editor/${properties.UNITY_VERSION}/Unity.app/Contents/MacOS/Unity -quit -buildTarget iOS -batchmode -projectPath . -executeMethod BuildScript.PerformiOSBuild"
            }
    
            stageName='Build XCode project'
            stage(stageName){
                dir('builds/iOS') {
                    echo "N1"
                    withCredentials([usernamePassword(credentialsId: 'ROOT_PWD', passwordVariable: 'ROOT_PASSWORD')]) {
                    echo "N2"
                        sh 'security unlock-keychain -p $ROOT_PASSWORD ~/Library/Keychains/login.keychain'
                    echo "N3"
                    }
                    sh "xcodebuild -project Unity-iPhone.xcodeproj -scheme Unity-iPhone -configuration Release CODE_SIGN_STYLE=Automatic DEVELOPMENT_TEAM=5CS6V7W342 -archivePath ../../${properties.PROJECT_NAME}-${properties.VERSION}.xcarchive archive"
                }
            }
        }
        
        echo "ARCHIVE"
        // Upload the final artifact (Binaries) to GitHub
        // Ioannis since i haven't build the final XCode project (you must insert your build script above)
        // i just zip and upload the build dir. I know that this is wrong, it's just a placeholder
        stageName='Archive artifacts'
        stage(stageName){
            sh "zip -r ${properties.VERSION}.zip ${properties.PROJECT_NAME}-${properties.VERSION}.xcarchive"
        }


        stageName='Upload to Artifactory'
        stage(stageName){
            upload2Artifactory(properties.PROJECT_NAME)
        }

        /* Upload release to Github
        // export needed tokens for github-release tool
        withCredentials([usernamePassword(credentialsId: 'testshock', passwordVariable: 'GITHUB_TOKEN')]) {
          
            stageName='Delete release'
            sh "/usr/local/bin/github-release delete --user ${properties.GITHUB_ORGANIZATION} --repo ${properties.GITHUB_REPO} --tag ${properties.VERSION_NAME}"
        
            stageName='Create release'
            sh "/usr/local/bin/github-release release --user ${properties.GITHUB_ORGANIZATION} --repo ${properties.GITHUB_REPO} --tag ${properties.VERSION_NAME} --name ${properties.VERSION_NAME}"

            stageName='Upload release'
            sh '/usr/local/bin/github-release upload --user ${properties.GITHUB_ORGANIZATION} --repo ${properties.GITHUB_REPO} --tag ${properties.VERSION_NAME} --name "${properties.PROJECT_NAME}-${properties.VERSION_NAME}.zip" --file artifacts.zip'
        }
        */

        notifyFinished(properties.PROJECT_NAME,properties.VERSION,env.BRANCH_NAME,GIT_COMMIT_SHORT)

    } catch (e){
        currentBuild.result='FAILURE'
        errorMessage="Failed on stage "+stageName
        notifyFailed(stageName,e,properties.PROJECT_NAME,properties.VERSION,env.BRANCH_NAME,GIT_COMMIT_SHORT)
    }
}

def createVersionFile(properties,gitCommitShort){

    STR = ""
    HASH = "HASH='${gitCommitShort}'"
    BUILDDATE = "BUILDDATE='${BUILD_DATE}'"
    BUILDTIME = "BUILDTIME='${BUILD_TIME}'"
    //TAG_NAME = "TAG='${TAG_NAME}'"
    VTAG = "VTAG='${env.BRANCH_NAME}'"
    VERSION_NAME = "VERSIONNAME=${properties.VERSION_NAME}"
    VERSION = "VERSION=${properties.VERSION}"
    COMMENTS = "COMMENTS=${properties.COMMENT}"

    STR = VERSION+"\n"+VERSION_NAME+"\n"+VTAG+"\n"+HASH+"\n"+BUILDDATE+"\n"+BUILDTIME+"\n"+COMMENTS

    writeFile file: 'version.properties', text: STR

    // Generate Jenkins description info
    currentBuild.description = VTAG+" H:"+HASH+" V:"+VERSION

}

def notifyFinished(projectName,version,branchName,hash){
    slackSend botUser: true, channel: 'test-notification', color: '#00ff00', message: projectName+" Version:"+version+" Branch:"+branchName+" Success", tokenCredentialId: 'slack-jenkins-id'
}

def notifyFailed(stageName,e,projectName,version,branchName,hash) {
    echo "Failed while in Stage ${stageName}"
    slackSend botUser: true, channel: 'test-notification', color: '#FF0000', message: projectName+" Version:"version+" Branch:"+branchName+" Failed in Stage:"+stageName, tokenCredentialId: 'slack-jenkins-id'
    echo e
}

def upload2Artifactory(projectName){
  url = "${projectName}/"
  stage("Upload REST APIs to Artifactory "+url) {
    proben: {
          def server = Artifactory.server 'testshock-af'
          def uploadSpec = """{
               "files": [
                {
                    "pattern": "*.zip",
                    "target": "${url}"
                  }
               ]
              }"""

          try {
              def buildInfo = Artifactory.newBuildInfo()
              server.upload spec: uploadSpec, buildInfo: buildInfo
              server.publishBuildInfo buildInfo
          }catch (e){
              errorMessage='Upload to Artifactory failed';
              notifyFailed(errorMessage,e);
              return;                    
          }
//        notifyEmail("Uploaded on Artifactory","Good Job!")
    }
  }
}
