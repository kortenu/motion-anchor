package dev.kortenu.motionanchor;

import com.mojang.blaze3d.platform.InputConstants;
import net.fabricmc.api.ClientModInitializer;
import net.fabricmc.fabric.api.client.event.lifecycle.v1.ClientTickEvents;
import net.fabricmc.fabric.api.client.keymapping.v1.KeyMappingHelper;
import net.fabricmc.fabric.api.client.rendering.v1.hud.HudElementRegistry;
import net.minecraft.client.KeyMapping;
import net.minecraft.client.Minecraft;
import net.minecraft.client.renderer.RenderPipelines;
import net.minecraft.resources.Identifier;

public final class MotionAnchorClient implements ClientModInitializer {
    private static final int DOT_SIZE = 8;
    private static final int DOT_TEXTURE_SIZE = 32;
    private static final Identifier DOT_TEXTURE =
            Identifier.fromNamespaceAndPath("motion-anchor", "textures/gui/dot.png");
    private static final float[] DOT_COLUMN_POSITIONS = {0.076f, 0.316f, 0.698f, 0.923f};
    private static final float[] DOT_ROW_POSITIONS = {0.089f, 0.370f, 0.673f, 0.930f};
    private static boolean overlayEnabled = true;

    private static final KeyMapping.Category KEY_CATEGORY =
            KeyMapping.Category.register(Identifier.fromNamespaceAndPath("motion-anchor", "controls"));
    private static final KeyMapping TOGGLE_KEY = KeyMappingHelper.registerKeyMapping(
            new KeyMapping(
                    "key.motion-anchor.toggle",
                    InputConstants.Type.KEYBOARD,
                    InputConstants.KEY_O,
                    KEY_CATEGORY
            )
    );

    @Override
    public void onInitializeClient() {
        ClientTickEvents.END_CLIENT_TICK.register(client -> {
            while (TOGGLE_KEY.consumeClick()) {
                overlayEnabled = !overlayEnabled;
            }
        });

        HudElementRegistry.addLast(
                Identifier.fromNamespaceAndPath("motion-anchor", "dot_matrix"),
                MotionAnchorClient::renderDots
        );
    }

    private static void renderDots(net.minecraft.client.gui.GuiGraphicsExtractor graphics,
                                   net.minecraft.client.DeltaTracker deltaTracker) {
        if (!overlayEnabled) {
            return;
        }

        var window = Minecraft.getInstance().getWindow();
        int width = window.getGuiScaledWidth();
        int height = window.getGuiScaledHeight();
        for (float rowPosition : DOT_ROW_POSITIONS) {
            int y = Math.round(rowPosition * height - DOT_SIZE / 2.0f);
            for (float columnPosition : DOT_COLUMN_POSITIONS) {
                int x = Math.round(columnPosition * width - DOT_SIZE / 2.0f);
                graphics.blit(
                        RenderPipelines.GUI_TEXTURED,
                        DOT_TEXTURE,
                        x,
                        y,
                        0,
                        0,
                        DOT_SIZE,
                        DOT_SIZE,
                        DOT_TEXTURE_SIZE,
                        DOT_TEXTURE_SIZE
                );
            }
        }
    }
}
