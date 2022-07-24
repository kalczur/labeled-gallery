import * as React from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ProvideAuth } from "./src/hooks/useAuth";
import AppRoutes from "./src/components/AppRoutes";

const queryClient = new QueryClient();

const App = () => {
  return (
    <QueryClientProvider client={ queryClient }>
      <ProvideAuth>
        <AppRoutes />
      </ProvideAuth>
    </QueryClientProvider>
  );
};

export default App;
