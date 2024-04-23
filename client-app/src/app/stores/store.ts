import { createContext, useContext } from "react";
import ActivityStores from "./activityStores";
import CommonStore from "./commonStore";
import UserStore from "./userStores";
import ModalStore from "./modalStore";

interface Store {
    activityStore: ActivityStores,
    commonStore: CommonStore,
    userStore: UserStore
    modalStore: ModalStore
}

export const store: Store = {
    activityStore: new ActivityStores(),
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}