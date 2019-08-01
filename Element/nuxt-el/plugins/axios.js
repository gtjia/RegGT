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
            //console.log(error.response.data)
        }
        const code = parseInt(error.response && error.response.status)
        if (code === 400) {
            redirect('/400')
        } else if (code === 401) {
            console.log(error)
            if(error.response.headers["refresh-token"])
            {
                let token = $nuxt.$auth.strategies["local"].options.tokenType + ' ' + error.response.headers["refresh-token"]
                $nuxt.$auth.setToken("local", token)
                $nuxt.$auth.mounted()
                
                error.config.headers["Authorization"] = token
                return $axios.request(error.config);
            }
        } else if (code === 500) {
            redirect('/sorry')
        }
    })
}