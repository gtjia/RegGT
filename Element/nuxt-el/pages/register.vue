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
                            <el-form-item label="确认密码" prop="checkPass">
                                <el-input type="password" v-model="loginForm.checkPass" auto-complete="off"></el-input>
                            </el-form-item>
                            <el-form-item>
                                <el-button type="primary" @click="submitForm('loginForm')">提交</el-button>
                                <el-button @click="resetForm('loginForm')">重置</el-button>
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
        var validatePass2 = (rule, value, callback) => {
            if (value !== this.loginForm.pass){
                callback(new Error('两次数据密码不一致!'));
            } else {
                callback();
            }
        };
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
                ],
                checkPass: [
                    { required: true, message: '请再次输入密码', trigger: 'blur'},
                    { validator: validatePass2, trigger: 'blur' }
                ]
            }
        }
    },
    methods: {
        submitForm(formName){
            this.$refs[formName].validate((valid) => {
                if(valid) {
                    console.log(this.loginForm.username);
                    this.$auth.loginWith('local', {
                        data: {
                            UserName: this.loginForm.username,
                            Password: this.loginForm.pass
                        }
                    }).then(() => { 
                        this.$message('Logged In!');
                        })
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        },
        resetForm(formName){
            console.log(this.$auth.user);
            this.$refs[formName].resetFields();
        }
    }
})
</script>

<style>
  .box-card {
    width: 480px;
  }
</style>