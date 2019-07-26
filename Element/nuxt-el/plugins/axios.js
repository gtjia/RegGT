export default function ({
    $axios,
    redirect,
    isDev
}) {
    $axios.onRequest(config => {
        console.log('Making request to ' + config.url)
    })
    $axios.onResponse(config => {
        console.log(config)
    })
    $axios.onError(error => {
        if(isDev)
        {
            console.log(error.response.data)
        }
        const code = parseInt(error.response && error.response.status)
        if (code === 400) {
            redirect('/400')
        } else if (code === 401) {
            console.log(error)
            $axios.post("/api/auth/refresh", {}).then((res) => {
                let token = $nuxt.$auth.strategies["local"].options.tokenType + ' ' + res.data.token
                $nuxt.$auth.setToken("local", token)
                $nuxt.$auth.mounted()
                
                return $axios.request(error.config);
            })
            //$nuxt.$auth.reset()
        } else if (code === 500) {
            redirect('/sorry')
        }
    })
}