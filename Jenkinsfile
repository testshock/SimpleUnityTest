//@Library('jenkins-shared-library') _
node('NativeMacOSJenkins') {
    // Checkout code
    checkout scm

    // Load static properties from config.properties
    properties = readProperties file: 'config.properties'

    // Generate version file with static and dynamic info
    createVersionFile(properties)

    // Global Exception handling.
    try {
        echo "BUILD"

        // Build iOS Unity project IF key iOS='TRUE' (in config.properties file)

        if (properties.iOS == 'TRUE') {
            stageName='Build Unity iOS'
            stage(stageName){
                sh "/Applications/Unity/Hub/Editor/${properties.unity}/Unity.app/Contents/MacOS/Unity -quit -buildTarget iOS -batchmode -projectPath . -executeMethod BuildScript.PerformiOSBuild"
            }
    
            stageName='Build XCode project'
            stage(stageName){
                echo "Ioannis insert your build code here"
            }
        }
        
        echo "ARCHIVE"
        // Upload the final artifact (Binaries) to GitHub
        // Ioannis since i haven't build the final XCode project (you must insert your build script above)
        // i just zip and upload the build dir. I know that this is wrong, it's just a placeholder
        stageName='Archive artifacts'
        stage(stageName){
            sh "zip -r artifacts.zip bulds"
        }


        // export needed tokens for github-release tool
        environment {
                GITHUB_TOKEN = credentials('cmd')
        }
        stageName='Delete release'
        
        stageName='Create release'

        stageName='Upload release'


        echo "SLACK"
    } catch (e){
        currentBuild.result='FAILURE';
        errorMessage("Failed on stage "+stageName)
        notifyFailed(stageName,e)
    }
}

def createVersionFile(properties){
        GIT_COMMIT_SHORT = sh(
        script: "git rev-parse --short=6 HEAD",
        returnStdout: true
    )

    GIT_COMMIT_SHORT = GIT_COMMIT_SHORT.replaceAll("\n","")

    STR = ""
    HASH = "HASH='${GIT_COMMIT_SHORT}'"
    BUILDDATE = "BUILDDATE='${BUILD_DATE}'"
    BUILDTIME = "BUILDTIME='${BUILD_TIME}'"
    //TAG_NAME = "TAG='${TAG_NAME}'"
    VTAG = "VTAG='${env.BRANCH_NAME}'"
    VERSION_NAME = "VERSIONNAME=${properties.versionname}"
    VERSION = "VERSION=${properties.version}"
    COMMENTS = "COMMENTS=${properties.comments}"

    STR = VERSION+"\n"+VERSION_NAME+"\n"+VTAG+"\n"+HASH+"\n"+BUILDDATE+"\n"+BUILDTIME+"\n"+COMMENTS

    writeFile file: 'version.properties', text: STR

    // Generate Jenkins description info
    currentBuild.description = VTAG+" H:"+HASH+" V:"+VERSION

}

def notifyFailed(stageName,e) {
    echo "Failed while in Stage ${stageName}"
    echo e
}
