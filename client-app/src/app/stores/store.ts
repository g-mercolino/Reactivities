import { createContext, useContext } from "react";
import ActivityStores from "./activityStores";

interface Store {
    activityStore: ActivityStores
}

export const store: Store = {
    activityStore: new ActivityStores()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}