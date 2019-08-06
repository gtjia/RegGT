<template>
    <el-container>
        <el-header>Header</el-header>
        <el-main>
            <el-row type="flex" class="row-bg" justify="center" align="middle">
                <el-col :span="6">
                    <el-card class="box-card">
                        <div slot="header" class="clearfix center">
                            <span>登陆</span>
                        </div>
                        <el-form :model="loginForm" ref="loginForm" status-icon :rules="rules" label-width="100px" class="demo-ruleForm">
                            <el-form-item label="用户名" prop="username">
                                <el-input type="text" v-model="loginForm.username" auto-complete="off"></el-input>
                            </el-form-item>
                            <el-form-item label="密码" prop="pass">
                                <el-input type="password" v-model="loginForm.pass" auto-complete="off"></el-input>
                            </el-form-item>
                            <el-form-item>
                                <el-button type="primary" @click="submitForm('loginForm')">提交</el-button>
                                <el-button type="primary" @click="testGet()">测试</el-button>
                            </el-form-item>
                        </el-form>
                    </el-card>
                </el-col>
            </el-row>
        </el-main>
    </el-container>
</template>
<script lang="js">
import Vue from 'vue'
export default Vue.extend({
    data(){
        return{
            loginForm: {
                username: '',
                pass: '',
                checkPass: ''
            },
            rules: {
                username: [
                    { required: true, message: '请输入用户名', trigger: 'blur' }
                ],
                pass: [
                    { required: true, message: '请输入密码', trigger: 'blur'}
                ]
            }
        }
    },
    methods: {
        submitForm(formName){
            this.$refs[formName].validate((valid) => {
                if(valid) {
                    /*
                    this.$auth.loginWith('local', {
                        data: {
                            UserName: this.loginForm.username,
                            Password: this.loginForm.pass
                        }
                    }).then(() => { 
                        this.$message('Logged In!');
                        })
                    */
                   this.$axios.post('/api/auth/Login', {
                        UserName: this.loginForm.username,
                        Password: this.loginForm.pass
                    }).then(
                        (res) => {
                            if(res.data.error) {
                                this.$message.error(res.data.error);
                                //this.$alert(res.data.error, "错误", {type: "error", showIcon: true});
                            }
                            else {
                                this.$auth.setUserToken(res.data.data)
                            }
                        }, 
                        (reason, data) => {
                            console.log('rejected失败回调');
                            console.log('失败执行回调抛出失败原因：',reason);
                        }
                    )
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        },
        testGet(){
            this.$axios.get("/api/values").then((res) => {
                console.log(res.data);
            })
        }
    }
})
</script>

<style>
  .box-card {
    width: 480px;
  }
</style>