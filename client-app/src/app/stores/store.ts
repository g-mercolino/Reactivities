import { createContext, useContext } from "react";
import ActivityStores from "./activityStores";
import CommonStore from "./commonStore";

interface Store {
    activityStore: ActivityStores,
    commonStore: CommonStore
}

export const store: Store = {
    activityStore: new ActivityStores(),
    commonStore: new CommonStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}