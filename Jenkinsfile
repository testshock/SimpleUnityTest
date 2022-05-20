//@Library('jenkins-shared-library') _
node('NativeMacOSJenkins') {
    checkout scm
    createVersionFile()

    //if (params.build){
        stage('Build'){
            //build-unity(UNITYVERSION, TARGET,'Test2D')
            sh "/Applications/Unity/Hub/Editor/2020.3.30f1/Unity.app/Contents/MacOS/Unity -quit -buildTarget iOS -batchmode -projectPath . -executeMethod BuildScript.PerformiOSBuild"
        }
    //}
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
    VTAG = "export const VTAG = '${env.BRANCH_NAME}';"

    STR = VTAG+'\n'+"\n"+HASH+"\n"+BUILDDATE+"\n"+BUILDTIME

    writeFile file: 'version.ts', text: STR
}
