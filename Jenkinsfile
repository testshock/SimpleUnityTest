//@Library('jenkins-shared-library') _
node('NativeMacOSJenkins') {
    if (params.checkout){
        stage('Checkout and Build') {
            echo "Going for "+BRANCH
            git branch: BRANCH, credentialsId:'github-user', url: 'https://github.com/testshock/Test2D.git'
            echo "Finished"
            createVersionFile()
        }
    }

    if (params.build){
        stage('Build'){
            //build-unity(UNITYVERSION, TARGET,'Test2D')
            sh "/Applications/Unity/Hub/Editor/${UNITYVERSION}/Unity.app/Contents/MacOS/Unity -quit -buildTarget ${BUILDTARGET} -batchmode -projectPath . -executeMethod BuildScript.Perform${BUILDTARGET}Build"
        }
    }
}

def createVersionFile(){
        GIT_COMMIT_SHORT = sh(
        script: "git rev-parse --short=6 HEAD",
        returnStdout: true
    )

    GIT_COMMIT_SHORT = GIT_COMMIT_SHORT.replaceAll("\n","")

    STR = ""
    HASH = "export const HASH = '${GIT_COMMIT_SHORT}';"
    BUILDDATE = "export const BUILDDATE = '${BUILD_DATE}';"
    BUILDTIME = "export const BUILDTIME = '${BUILD_TIME}';"
    VTAG = "export const VTAG = '${BRANCH}';"

    STR = VTAG+'\n'+"\n"+HASH+"\n"+BUILDDATE+"\n"+BUILDTIME

    writeFile file: 'version.ts', text: STR
}

def createDevVersionFile(){
    GIT_COMMIT_SHORT = sh(
        script: "git rev-parse --short=6 HEAD",
        returnStdout: true
    )
    GIT_COMMIT_SHORT = GIT_COMMIT_SHORT.replaceAll("\n","")

    // crreate version.ts file
    STR = ""
    HASH = "export const HASH = '${GIT_COMMIT_SHORT}';"
    BUILDDATE = "export const BUILDDATE = '${BUILD_DATE}';"
    BUILDTIME = "export const BUILDTIME = '${BUILD_TIME}';"
    VTAG = "export const VTAG = '0';"
    DISTANCE = "export const DISTANCE = 'not implemented';"
    STR = VTAG+'\n'+DISTANCE+"\n"+HASH+"\n"+BUILDDATE+"\n"+BUILDTIME

    writeFile file: 'version.ts', text: STR
}
def createTagedVersionFile(){
    GIT_COMMIT_SHORT = sh(
        script: "git rev-parse --short=6 HEAD",
        returnStdout: true
    )

    GIT_COMMIT_SHORT = GIT_COMMIT_SHORT.replaceAll("\n","")

    STR = ""
    HASH = "export const HASH = '${GIT_COMMIT_SHORT}';"
    BUILDDATE = "export const BUILDDATE = '${BUILD_DATE}';"
    BUILDTIME = "export const BUILDTIME = '${BUILD_TIME}';"
    VTAG = "export const VTAG = '${GITTAG}';"
    DISTANCE = "export const DISTANCE = 'not implemented';"
    STR = VTAG+'\n'+DISTANCE+"\n"+HASH+"\n"+BUILDDATE+"\n"+BUILDTIME

    writeFile file: 'version.ts', text: STR
}
