const headers = {
  namespaced: true,
  state: {
    userCurrent: {}
  },
  mutations: {
    SetCurrentUser: (state) => {
      state.userCurrent = {
        name: 'ε«δΈθε',
        avatar: ''
      }
    }
  },
  actions: {
    getCurrentUser({
      commit
    }) {
      commit("SetCurrentUser")
    }
  }

};

export default headers;